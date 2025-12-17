using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Domains;

public record PositionedTag(Tag Tag, Rectangle Rectangle, float FontSize);