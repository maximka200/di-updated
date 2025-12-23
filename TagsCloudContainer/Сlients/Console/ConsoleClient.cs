using Autofac;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.OutputFormats;
using TagsCloudContainer.Сlients.Console.Parsing;
using TagsCloudContainer.Сlients.Interfaces;
using Point = SixLabors.ImageSharp.Point;
using Size = SixLabors.ImageSharp.Size;

namespace TagsCloudContainer.Сlients.Console;

public class ConsoleClient(ILifetimeScope root) : IClient
{
    public int Run(string[] args)
    {
        if (!ConsoleOptionsParser.TryParse(args, out var o, out var error))
        {
            if (error != "help")
                System.Console.WriteLine(error);

            PrintUsage();
            return 1;
        }

        if (!File.Exists(o?.StopWordsPath))
        {
            System.Console.WriteLine($"Файл стоп-слов не найден: {o?.StopWordsPath}");
            return 1;
        }

        try
        {
            using var scope = root.BeginLifetimeScope(b =>
                b.RegisterModule(new TagCloudBuilder(
                    new Point(o.CenterX, o.CenterY),
                    o.StopWordsPath, OutputFormatSupport.Sources.ToArray(),
                    SourceFormatSupport.Sources.ToArray())
                ));

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

    private static TagCloudGenerationRequest BuildRequest(ConsoleOptions? o) =>
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
            TextColor = o.TextColor,
            BackgroundColor = o.BackgroundColor,
            Font = o.Font,
            Desc = o.Desc
        };

    private static void PrintUsage()
    {
        System.Console.WriteLine("Console client flags:");
        System.Console.WriteLine("  --input <path> [--output cloud.png]");
        System.Console.WriteLine("  [--width 800] [--height 800] [--center-x width/2] [--center-y height/2]");
        System.Console.WriteLine("  [--stop-words ./stop-words.txt]");
        System.Console.WriteLine("  [--min-font 10] [--max-font 60] [--source-type txt]");
        System.Console.WriteLine("  [--bg #ffffff] [--fg #000000] [--font-family monospace|\"Arial\"|./fonts/Roboto.ttf]");
        System.Console.WriteLine("  [--format png|jpg|jpeg|bmp]");
        System.Console.WriteLine("  [--desc false|true]");
    }
}
