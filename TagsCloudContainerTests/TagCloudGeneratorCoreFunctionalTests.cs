using Autofac;
using FluentAssertions;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.OutputFormats;
using TagsCloudContainer.Core.WordSources;

namespace TagsCloudContainerTests;

[TestFixture]
public class TagCloudGeneratorCoreFunctionalTests
{
    [Test]
    public void Generate_FromTxtToPng_ShouldCreateNotEmptyPngFile()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        var inputPath = Path.Combine(tempDir.FullName, "words.txt");
        var outputPath = Path.Combine(tempDir.FullName, "cloud.png");

        File.WriteAllLines(inputPath, [
            "Hello",
            "world",
            "hello",
            "Cloud",
            "cloud",
            "the",      
            "and"
        ]);

        const int width = 800;
        const int height = 600;

        using var container = BuildTestContainer("stop-words.txt", width, height);
        var generator = container.Resolve<ITagCloudGenerator>();

        var request = CreateRequest(
            inputPath: inputPath,
            outputPath: outputPath,
            width: width,
            height: height,
            outputFormat: "png",
            textColor: Color.Black,
            backgroundColor: Color.White,
            12f,
            64f,
            "txt"
        );
        
        generator.Generate(request);
        
        File.Exists(outputPath).Should().BeTrue();

        var bytes = File.ReadAllBytes(outputPath);
        bytes.Should().NotBeNull();
        bytes.Length.Should().BeGreaterThan(100);
        
        var pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }; // PNG signature: 89 50 4E 47 0D 0A 1A 0A
        bytes.Take(8).Should().Equal(pngHeader);
    }
    
    [Test]
    public void Generate_FromTxtToPng_ShouldCreatePngWithExpectedDimensions()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = Path.Combine(tempDir.FullName, "words.txt");
            var outputPath = Path.Combine(tempDir.FullName, "cloud.png");
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, ["the", "and"]);
            File.WriteAllLines(inputPath, ["Hello", "world", "hello", "Cloud", "cloud"]);

            const int width = 640;
            const int height = 480;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: inputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "png",
                textColor: Color.Black,
                backgroundColor: Color.White,
                12f,
                64f,
                "txt");

            generator.Generate(request);

            using var image = Image.Load<Rgba32>(outputPath);
            image.Width.Should().Be(width);
            image.Height.Should().Be(height);
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void Generate_FromTxtToPng_ShouldApplyBackgroundColor()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = Path.Combine(tempDir.FullName, "words.txt");
            var outputPath = Path.Combine(tempDir.FullName, "cloud.png");
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, new[] { "the", "and" });
            File.WriteAllLines(inputPath, new[] { "one", "two", "three", "two", "three", "three" });

            const int width = 800;
            const int height = 600;

            var bg = Color.AliceBlue;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: inputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "png",
                textColor: Color.Black,
                backgroundColor: bg,
                12f,
                64f,
                "txt");

            generator.Generate(request);

            using var image = Image.Load<Rgba32>(outputPath);
            
            image[0, 0].Should().Be(bg.ToPixel<Rgba32>());
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void Generate_FromTxtToPng_ShouldDrawSomethingNotEqualToBackground()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = Path.Combine(tempDir.FullName, "words.txt");
            var outputPath = Path.Combine(tempDir.FullName, "cloud.png");
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, ["the", "and"]);
            File.WriteAllLines(inputPath, ["alpha", "beta", "beta", "gamma", "gamma", "gamma"]);

            const int width = 900;
            const int height = 500;

            var bg = Color.White;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: inputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "png",
                textColor: Color.Black,
                backgroundColor: bg,
                12f,
                64f,
                "txt");

            generator.Generate(request);

            using var image = Image.Load<Rgba32>(outputPath);

            HasAnyNonBackgroundPixel(image, bg.ToPixel<Rgba32>())
                .Should()
                .BeTrue("облако слов должно содержать пиксели, отличные от фона");
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void Generate_WithOutputFormatPngAndJpgExtension_ShouldCreatePngByHeader()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = Path.Combine(tempDir.FullName, "words.txt");
            var outputPath = Path.Combine(tempDir.FullName, "cloud.jpg"); 
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, ["the", "and"]);
            File.WriteAllLines(inputPath, ["hello", "hello", "world"]);

            const int width = 700;
            const int height = 700;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: inputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "png",
                textColor: Color.Black,
                backgroundColor: Color.White,
                12f,
                64f,
                "txt");

            generator.Generate(request);

            File.Exists(outputPath).Should().BeTrue();

            var bytes = File.ReadAllBytes(outputPath);
            bytes.Length.Should().BeGreaterThan(100);

            var pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            bytes.Take(8).Should().Equal(pngHeader);
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void Generate_WhenSourceFileDoesNotExist_ShouldThrowFileNotFoundException()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var missingInputPath = Path.Combine(tempDir.FullName, "missing.txt");
            var outputPath = Path.Combine(tempDir.FullName, "cloud.png");
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, ["the", "and"]);

            const int width = 800;
            const int height = 600;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: missingInputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "png",
                textColor: Color.Black,
                backgroundColor: Color.White,
                12f,
                64f,
                "txt");

            var act = () => generator.Generate(request);

            act.Should().Throw<FileNotFoundException>();
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void Generate_WhenOutputFormatIsUnsupported_ShouldThrow()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = Path.Combine(tempDir.FullName, "words.docs");
            var outputPath = Path.Combine(tempDir.FullName, "cloud.unknown");
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, ["the", "and"]);
            File.WriteAllLines(inputPath, ["hello", "world"]);

            const int width = 800;
            const int height = 600;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: inputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "definitely-not-a-format",
                textColor: Color.Black,
                backgroundColor: Color.White,
                12f, 
                64f,
                "txt");

            var act = () => generator.Generate(request);
            
            act.Should().Throw<NotSupportedException>();
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }
    
    [Test]
    public void Generate_WhenSourceFormatIsUnsupported_ShouldThrow()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = Path.Combine(tempDir.FullName, "words.docs");
            var outputPath = Path.Combine(tempDir.FullName, "cloud.unknown");
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, ["the", "and"]);
            File.WriteAllLines(inputPath, ["hello", "world"]);

            const int width = 800;
            const int height = 600;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: inputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "png",
                textColor: Color.Black,
                backgroundColor: Color.White,
                12f, 
                64f,
                "no-support");

            var act = () => generator.Generate(request);
            
            act.Should().Throw<NotSupportedException>();
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }
    
        [TestCase("arial")]
    [TestCase("helvetica")]
    [TestCase("menlo")]
    public void Generate_WithSupportedFontChoice_ShouldCreateNotEmptyPngFile(string fontChoice)
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = Path.Combine(tempDir.FullName, "words.txt");
            var outputPath = Path.Combine(tempDir.FullName, $"cloud-{fontChoice}.png");
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, ["the", "and"]);
            File.WriteAllLines(inputPath, ["Hello", "world", "hello", "Cloud", "cloud"]);

            const int width = 800;
            const int height = 600;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: inputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "png",
                textColor: Color.Black,
                backgroundColor: Color.White,
                12f,
                64f,
                "txt",
                fontChoice);

            generator.Generate(request);

            File.Exists(outputPath).Should().BeTrue();

            var bytes = File.ReadAllBytes(outputPath);
            bytes.Should().NotBeNull();
            bytes.Length.Should().BeGreaterThan(100);

            var pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            bytes.Take(8).Should().Equal(pngHeader);
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }

    [Test]
    public void Generate_WhenFontChoiceIsUnsupported_ShouldThrow()
    {
        var tempDir = Directory.CreateTempSubdirectory("tags-cloud-tests-");
        try
        {
            var inputPath = Path.Combine(tempDir.FullName, "words.txt");
            var outputPath = Path.Combine(tempDir.FullName, "cloud.png");
            var stopWordsPath = Path.Combine(tempDir.FullName, "stop-words.txt");

            File.WriteAllLines(stopWordsPath, ["the", "and"]);
            File.WriteAllLines(inputPath, ["Hello", "world", "hello"]);

            const int width = 800;
            const int height = 600;

            using var container = BuildTestContainer(stopWordsPath, width, height);
            var generator = container.Resolve<ITagCloudGenerator>();

            var request = CreateRequest(
                inputPath: inputPath,
                outputPath: outputPath,
                width: width,
                height: height,
                outputFormat: "png",
                textColor: Color.Black,
                backgroundColor: Color.White,
                12f,
                64f,
                "txt",
                "definitely-not-a-font");

            var act = () => generator.Generate(request);

            act.Should().Throw<NotSupportedException>();
        }
        finally
        {
            TryDeleteDirectory(tempDir.FullName);
        }
    }


    private static IContainer BuildTestContainer(string stopWordsPath, int width, int height)
    {
        var builder = new ContainerBuilder();

        IOutputFormat[] outputFormats = [
            new PngOutputSource(), 
            new JpegOutputSource(), 
            new JpegOutputSource()
        ];
        
        IWordsSource[] inputSources =
        [
            new TxtWordsSource(),
            new DocWordsSource(),
            new DocxWordsSource()
        ];
    
        builder.RegisterModule(new TagCloudBuilder(
            center: new Point(width / 2, height / 2),
            stopWordsPath: stopWordsPath, outputFormats, inputSources));

        return builder.Build();
    }

    private static TagCloudGenerationRequest CreateRequest(
        string inputPath,
        string outputPath,
        int width,
        int height,
        string outputFormat,
        Color textColor,
        Color backgroundColor,
        float minFontSize,
        float maxFontSize,
        string sourceFormat,
        string? font = null)
    {
        return new TagCloudGenerationRequest
        {
            SourceSettings = new SourceSettings(inputPath, sourceFormat),
            LayoutSettings = new LayoutSettings
            {
                ImageSize = new Size(width, height),
                MinFontSize = minFontSize,
                MaxFontSize = maxFontSize
            },
            OutputPath = outputPath,
            OutputFormat = outputFormat,
            TextColor = textColor,
            BackgroundColor = backgroundColor,
            Font = font ?? GetAnyFontName() 
        };
    }

    private static string? GetAnyFontName()
    {
        return FontFamilyResolver.Choices.FirstOrDefault();
    }

    private static bool HasAnyNonBackgroundPixel(Image<Rgba32> image, Rgba32 background)
    {
        for (var y = 0; y < image.Height; y += 8)
        for (var x = 0; x < image.Width; x += 8)
        {
            if (image[x, y] != background)
                return true;
        }

        return false;
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            Directory.Delete(path, recursive: true);
        }
        catch
        {
            // ignored
        }
    }
}
