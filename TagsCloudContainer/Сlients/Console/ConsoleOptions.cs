using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console;

public sealed record ConsoleOptions(
    string InputPath,
    string OutputPath,
    int Width,
    int Height,
    int CenterX,
    int CenterY,
    string StopWordsPath,
    float MinFontSize,
    float MaxFontSize,
    string SourceType,
    string OutputFormat,
    Color BackgroundColor,
    Color TextColor,
    string FontFamily
);