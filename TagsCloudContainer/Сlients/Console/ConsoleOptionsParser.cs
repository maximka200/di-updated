using TagsCloudContainer.Сlients.Console.Parsing;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using TagsCloudContainer.Сlients.Console.Parsing.ParseResults;
using TagsCloudContainer.Сlients.Console.Parsing.PreCheckResults;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console;

internal static class ConsoleOptionsParser
{
    private const int BaseWidth = 800;
    private const int BaseHeight = 800;

    private static readonly IPreCheck[] PreChecks =
    [
        new EmptyArgsIsHelpPreCheck(),
        new HelpFlagPreCheck("-h"),
        new HelpFlagPreCheck("--help"),
    ];

    public static bool TryParse(string[] args, out ConsoleOptions? options, out string error) =>
        Parse(args).Apply(out options, out error);

    private static ParseResult Parse(string[] args)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(args);

            var pre = PreChecks.Aggregate(
                PreCheckResult.Continue,
                (acc, next) => acc.OrElse(() => next.Check(args)));

            return pre.Then(() => ParseCore(args));
        }
        catch (Exception e)
        {
            return ParseResult.Fail(e.Message);
        }
    }

    private static ParseResult ParseCore(string[] args)
    {
        var flags = FlagsParser.Parse(args);
        var r = new FlagReader(flags);

        var inputPath = r.RequirePath(CliFlags.Input, "Входной файл");
        var outputPath = r.GetString(CliFlags.Output, "cloud.png");

        var width = r.GetInt(CliFlags.Width, BaseWidth, new PositiveIntRule(CliFlags.Width));
        var height = r.GetInt(CliFlags.Height, BaseHeight, new PositiveIntRule(CliFlags.Height));

        var centerX = r.GetInt(CliFlags.CenterX, width / 2, new NonNegativeIntRule(CliFlags.CenterX));
        var centerY = r.GetInt(CliFlags.CenterY, height / 2, new NonNegativeIntRule(CliFlags.CenterY));

        var stopWordsPath = r.GetString(CliFlags.StopWords, Path.Combine(AppContext.BaseDirectory, "stop-words.txt"));
        stopWordsPath = Path.GetFullPath(stopWordsPath);

        var minFont = r.GetFloat(CliFlags.MinFont, 10f, new PositiveFloatRule(CliFlags.MinFont));
        var maxFont = r.GetFloat(CliFlags.MaxFont, 60f, new PositiveFloatRule(CliFlags.MaxFont));
        Ensure.True(minFont <= maxFont, "Некорректные значения шрифтов: min > max");

        var sourceType = r.GetString(CliFlags.SourceType, "txt");

        var bg = r.GetColor(CliFlags.Bg, Color.White);
        var fg = r.GetColor(CliFlags.Fg, Color.Black);

        var outputFormat = r.GetString(CliFlags.Format, OutputFormatSupport.FormatFromPath(outputPath));
        OutputFormatSupport.EnsureFormatSupported(outputFormat);

        var fontFamily = r.GetString(CliFlags.FontFamily, "arial");

        var options = new ConsoleOptions(
            InputPath: inputPath,
            OutputPath: outputPath,
            Width: width,
            Height: height,
            CenterX: centerX,
            CenterY: centerY,
            StopWordsPath: stopWordsPath,
            MinFontSize: minFont,
            MaxFontSize: maxFont,
            SourceType: sourceType,
            OutputFormat: outputFormat,
            BackgroundColor: bg,
            TextColor: fg,
            FontFamily: fontFamily
        );

        return ParseResult.Ok(options);
    }
}
