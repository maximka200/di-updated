using System.Drawing;
using Autofac;
using FluentAssertions;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainerTests;

[TestFixture]
public class TagCloudGeneratorFunctionalTests
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

        using var container = BuildTestContainer(width, height);
        var generator = container.Resolve<ITagCloudGenerator>();

        var request = new TagCloudGenerationRequest
        {
            SourceSettings = new SourceSettings(inputPath, "txt"),
            LayoutSettings = new LayoutSettings
            {
                ImageSize = new Size(width, height),
                MinFontSize = 12,
                MaxFontSize = 48
            },
            OutputPath = outputPath,
            OutputFormat = "png",
            BackgroundColor = Color.White,
            TextColor = Color.Black
        };
        
        generator.Generate(request);
        
        File.Exists(outputPath).Should().BeTrue();

        var bytes = File.ReadAllBytes(outputPath);
        bytes.Should().NotBeNull();
        bytes.Length.Should().BeGreaterThan(100);
        
        var pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }; // PNG signature: 89 50 4E 47 0D 0A 1A 0A
        bytes.Take(8).Should().Equal(pngHeader);
    }

    private static IContainer BuildTestContainer(int width, int height)
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterType<WordsSourceFactory>()
            .As<IWordsSourceFactory>()
            .SingleInstance();
        
        builder.RegisterType<LowerCaseNormalizer>()
            .As<IWordNormalizer>()
            .SingleInstance();

        builder.RegisterType<FileStopWordsProvider>()
            .As<IStopWordsProvider>()
            .SingleInstance();

        builder.RegisterType<StopWordsFilter>()
            .As<IWordsFilter>()
            .SingleInstance();

        builder.RegisterType<CompositeWordsPreprocessor>()
            .As<IWordsPreprocessor>()
            .SingleInstance();
        
        builder.RegisterType<WordFrequencyAnalyzer>()
            .As<IWordFrequencyAnalyzer>()
            .SingleInstance();
        
        builder.Register(_ =>
            new CircularCloudLayouterWrapper(new Point(width / 2, height / 2)))
            .As<ICircularCloudLayouterWrapper>()
            .SingleInstance();

        builder.RegisterType<TagSizeCalculator>()
            .As<ITagSizeCalculator>()
            .SingleInstance();

        builder.RegisterType<CloudPositionedTags>()
            .As<ICloudPositionedTags>()
            .WithParameter("minFontSize", 12f)
            .WithParameter("maxFontSize", 48f)
            .SingleInstance();
        
        builder.RegisterType<TagCloudGenerator>()
            .As<ITagCloudGenerator>()
            .SingleInstance();

        return builder.Build();
    }
}
