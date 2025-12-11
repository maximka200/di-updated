using System;
using System.Drawing;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using SixLabors.Fonts;
using FontFamily = SixLabors.Fonts.FontFamily;
using SystemFonts = SixLabors.Fonts.SystemFonts;

namespace TagsCloudContainer.Core;

public class TagSizeCalculator : ITagSizeCalculator
{
    private FontFamily fontFamily;

    public TagSizeCalculator(string fontName = "Arial")
    {
        if (!SystemFonts.TryGet(fontName, out fontFamily))
            fontFamily = SystemFonts.Collection.Families.First();
    }

    public Size GetSize(Tag tag, float fontSize)
    {
        var font = fontFamily.CreateFont(fontSize);

        var options = new TextOptions(font)
        {
            WrappingLength = float.PositiveInfinity
        };
        
        var rect = TextMeasurer.MeasureSize(tag.Word, options);

        return new Size(
            (int)Math.Ceiling(rect.Width),
            (int)Math.Ceiling(rect.Height));
    }
}