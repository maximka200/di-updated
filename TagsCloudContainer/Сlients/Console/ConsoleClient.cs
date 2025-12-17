using Autofac;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients.Console;

public sealed class ConsoleClient(ILifetimeScope root) : IClient
{
    public int Run(string[] args)
    {
        if (!TryParseArgs(args, out var o))
            return 1;

        try
        {
            if (!File.Exists(o.StopWordsPath))
            {
                System.Console.WriteLine($"Файл стоп-слов не найден: {o.StopWordsPath}");
                return 1;
            }

            var center = new Point(o.CenterX, o.CenterY);

            using var scope = root.BeginLifetimeScope(b =>
                b.RegisterModule(new TagCloudBuilder(center, o.StopWordsPath))
            );

            var generator = scope.Resolve<ITagCloudGenerator>();
            generator.Generate(BuildRequest(o));

            System.Console.WriteLine($"Облако тегов успешно сохранено в файл: {o.OutputPath}");
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

        if (args.Length > 7)
        {
            System.Console.WriteLine("Слишком много аргументов.");
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
        if (args.Length > 2 && (!int.TryParse(args[2], out width) || width <= 0))
        {
            System.Console.WriteLine("Некорректная ширина изображения");
            return false;
        }

        var height = 600;
        if (args.Length > 3 && (!int.TryParse(args[3], out height) || height <= 0))
        {
            System.Console.WriteLine("Некорректная высота изображения");
            return false;
        }

        var centerX = width / 2;
        if (args.Length > 4 && (!int.TryParse(args[4], out centerX) || centerX < 0))
        {
            System.Console.WriteLine("Некорректный centerX");
            return false;
        }

        var centerY = height / 2;
        if (args.Length > 5 && (!int.TryParse(args[5], out centerY) || centerY < 0))
        {
            System.Console.WriteLine("Некорректный centerY");
            return false;
        }

        var defaultStopWordsPath = Path.Combine(AppContext.BaseDirectory, "stop-words.txt");
        var stopWordsPath = args.Length > 6 ? args[6] : defaultStopWordsPath;
        if (string.IsNullOrWhiteSpace(stopWordsPath))
        {
            System.Console.WriteLine("Некорректный путь к файлу стоп-слов");
            return false;
        }

        var outputFormat = GetFormatFromPath(outputPath);

        options = new ConsoleOptions(
            InputPath: inputPath,
            OutputPath: outputPath,
            Width: width,
            Height: height,
            CenterX: centerX,
            CenterY: centerY,
            StopWordsPath: stopWordsPath,
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
        System.Console.WriteLine("  TagsCloudContainer <inputPath> [outputPath] [width] [height] [centerX] [centerY] [stopWordsPath]");
        System.Console.WriteLine();
        System.Console.WriteLine("Defaults:");
        System.Console.WriteLine("  outputPath=cloud.png, width=800, height=600, center=(width/2,height/2), stopWords=./stop-words.txt (рядом с exe)");
        System.Console.WriteLine();
        System.Console.WriteLine("Examples:");
        System.Console.WriteLine("  TagsCloudContainer input.txt");
        System.Console.WriteLine("  TagsCloudContainer input.txt cloud.png 800 600");
        System.Console.WriteLine("  TagsCloudContainer input.txt cloud.png 800 600 400 300 stop-words.txt");
    }
}
