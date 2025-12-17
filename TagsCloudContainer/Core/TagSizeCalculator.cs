using SixLabors.Fonts;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public sealed class TagSizeCalculator(FontFamily fontFamily) : ITagSizeCalculator
{
    private FontFamily fontFamily = fontFamily;
    private const int Padding = 5;

    public Size GetSize(Tag tag, float fontSize)
    {
        var font = fontFamily.CreateFont(fontSize);
        var bounds = TextMeasurer.MeasureBounds(tag.Word, new TextOptions(font)
        {
            WrappingLength = float.PositiveInfinity
        });
        
        return new Size(
            (int)(MathF.Ceiling(bounds.Width) + Padding),
            (int)(MathF.Ceiling(bounds.Height) + Padding)
        );
    }
}