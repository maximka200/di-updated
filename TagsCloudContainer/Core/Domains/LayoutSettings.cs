using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Domains;

public class LayoutSettings
{
    public Size ImageSize { get; init; }
    public required float MinFontSize { get; init; }
    public required float MaxFontSize { get; init; }
}