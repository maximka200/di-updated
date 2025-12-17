using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Domains;

public sealed record RenderContext(
    FontFamily FontFamily,
    float MinFontSize,
    float MaxFontSize,
    int MinFreq,
    int MaxFreq,
    Color TextColor
);