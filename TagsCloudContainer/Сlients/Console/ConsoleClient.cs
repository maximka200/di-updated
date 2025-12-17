using SixLabors.ImageSharp;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Сlients.Console;

public sealed class ConsoleClient(ITagCloudGenerator generator) : IClient
{
    public int Run(string[] args)
    {
        if (!TryParseArgs(args, out var options))
            return 1;

        try
        {
            generator.Generate(BuildRequest(options));
            System.Console.WriteLine($"Облако тегов успешно сохранено в файл: {options.OutputPath}");
            return 0;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Во время генерации облака произошла ошибка:");
            System.Console.WriteLine(ex);
            return 1;
        }
    }

    private static TagCloudGenerationRequest BuildRequest(ConsoleOptions o) =>
        new()
        {
            SourceSettings = new SourceSettings(o.InputPath, o.SourceType),
            LayoutSettings = new LayoutSettings
            {
                ImageSize = new Size(o.Width, o.Height),
                MinFontSize = o.MinFontSize,
                MaxFontSize = o.MaxFontSize
            },
            OutputPath = o.OutputPath,
            OutputFormat = o.OutputFormat,
            BackgroundColor = Color.White,
            TextColor = Color.Black
        };

    private static bool TryParseArgs(string[] args, out ConsoleOptions options)
    {
        options = default!;

        if (args.Length == 0 || args[0] is "-h" or "--help")
        {
            PrintUsage();
            return false;
        }

        var inputPath = args[0];
        if (string.IsNullOrWhiteSpace(inputPath))
        {
            System.Console.WriteLine("Некорректный путь к входному файлу");
            return false;
        }

        var outputPath = args.Length > 1 ? args[1] : "cloud.png";

        var width = 800;
        var height = 600;

        switch (args.Length)
        {
            case > 2 when !int.TryParse(args[2], out width):
                System.Console.WriteLine("Некорректная ширина изображения");
                return false;
            case > 3 when !int.TryParse(args[3], out height):
                System.Console.WriteLine("Некорректная высота изображения");
                return false;
        }

        var outputFormat = GetFormatFromPath(outputPath); 
        options = new ConsoleOptions(
            InputPath: inputPath,
            OutputPath: outputPath,
            Width: width,
            Height: height,
            MinFontSize: 10f,
            MaxFontSize: 60f,
            SourceType: "txt",
            OutputFormat: outputFormat
        );

        return true;
    }

    private static string GetFormatFromPath(string outputPath)
    {
        var ext = Path.GetExtension(outputPath);
        return string.IsNullOrWhiteSpace(ext) ? "png" : ext.TrimStart('.').ToLowerInvariant();
    }

    private static void PrintUsage()
    {
        System.Console.WriteLine("Usage:");
        System.Console.WriteLine("  TagsCloudContainer [inputPath] [outputPath] [width] [height]");
        System.Console.WriteLine();
        System.Console.WriteLine("Examples:");
        System.Console.WriteLine("  TagsCloudContainer input.txt");
        System.Console.WriteLine("  TagsCloudContainer input.txt cloud.png 800 600");
    }
}
