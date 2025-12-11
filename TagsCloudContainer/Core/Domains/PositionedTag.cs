using System.Drawing;

namespace TagsCloudContainer.Core.Domains;

public record PositionedTag(Tag Tag, Rectangle Rectangle, int FontSize);