using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console;

public record ConsoleOptions(
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
    string Font,
    bool Desc
);