using Autofac;
using FluentAssertions;
using SixLabors.ImageSharp;
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

        using var container = BuildTestContainer();
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

    private static IContainer BuildTestContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(new TagCloudBuilder(new Point(500, 500), "stop-words.txt"));
        return builder.Build();
    }
}
