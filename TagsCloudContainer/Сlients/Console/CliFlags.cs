namespace TagsCloudContainer.Сlients.Console;

internal static class CliFlags
{
    public const string Input = "--input";
    public const string Output = "--output";

    public const string Width = "--width";
    public const string Height = "--height";
    public const string CenterX = "--center-x";
    public const string CenterY = "--center-y";

    public const string StopWords = "--stop-words";

    public const string MinFont = "--min-font";
    public const string MaxFont = "--max-font";
    public const string SourceType = "--source-type";

    public const string Bg = "--bg";
    public const string Fg = "--fg";

    public const string FontFamily = "--font-family";
    public const string Format = "--format";

    public static readonly string[] All =
    [
        Input, Output,
        Width, Height, CenterX, CenterY,
        StopWords,
        MinFont, MaxFont, SourceType,
        Bg, Fg,
        FontFamily, Format
    ];
}