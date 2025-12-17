

using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Domains;

public class TagCloudGenerationRequest
{
    public SourceSettings SourceSettings { get; init; }
    public LayoutSettings LayoutSettings { get; init; }
    public string OutputPath { get; init; }
    public Color BackgroundColor { get; init; }
    public Color TextColor { get; init; }
    public string OutputFormat { get; init; }
}