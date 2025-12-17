namespace TagsCloudContainer.Сlients.Console;

public record ConsoleOptions(
    string InputPath,
    string OutputPath,
    int Width,
    int Height,
    float MinFontSize,
    float MaxFontSize,
    string SourceType,
    string OutputFormat
);