using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Domains;

public class TagCloudGenerationRequest
{
    public required SourceSettings SourceSettings { get; init; }
    public required LayoutSettings LayoutSettings { get; init; }
    public required string OutputPath { get; init; }
    public Color TextColor { get; init; }
    public Color BackgroundColor { get; init; }
    public string OutputFormat { get; init; }
    
    public string? Font { get; init; }
    
    public bool Desc { get; init; }
}