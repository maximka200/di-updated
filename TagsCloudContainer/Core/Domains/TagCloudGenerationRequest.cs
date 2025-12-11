using System.Drawing;

namespace TagsCloudContainer.Core.Domains;

public class TagCloudGenerationRequest
{
    public SourceSettings SourceSettings { get; init; } = null!;
    public LayoutSettings LayoutSettings { get; init; } = null!;
    public string OutputPath { get; init; } = "cloud.png";
    public Color BackgroundColor { get; init; } = Color.White;
    public Color TextColor { get; init; } = Color.Black;
    public string? StopWordsPath { get; init; }
    public string OutputFormat { get; init; } = "png";
}