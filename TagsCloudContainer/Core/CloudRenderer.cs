using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class CloudRenderer : ICloudRenderer
{
    public Image<Rgba32> Render(TagCloudGenerationRequest request, IReadOnlyCollection<PositionedTag> positionedTags)
    {
        var s = request.LayoutSettings;
        var img = new Image<Rgba32>(s.ImageSize.Width, s.ImageSize.Height, request.BackgroundColor);

        if (positionedTags.Count == 0)
            return img;

        var ctx = BuildRenderContext(request, positionedTags);

        img.Mutate(i =>
        {
            foreach (var (tag, rect, fontSize) in positionedTags)
            {
                var font = ctx.FontFamily.CreateFont(fontSize);
                var origin = GetCenteredOrigin(tag.Word, font, rect);
                var options = new RichTextOptions(font)
                {
                    Origin = origin,
                    WrappingLength = float.PositiveInfinity
                };

                i.DrawText(options, tag.Word, ctx.TextColor);
            }
        });

        return img;
    }

    private static RenderContext BuildRenderContext(
        TagCloudGenerationRequest request,
        IReadOnlyCollection<PositionedTag> positionedTags)
    {
        var s = request.LayoutSettings;

        var minFreq = positionedTags.Min(p => p.Tag.Frequency);
        var maxFreq = positionedTags.Max(p => p.Tag.Frequency);

        var fontFamily = FontFamilyResolver.Resolve(request.Font);

        return new RenderContext(
            FontFamily: fontFamily,
            MinFontSize: s.MinFontSize,
            MaxFontSize: s.MaxFontSize,
            MinFreq: minFreq,
            MaxFreq: maxFreq,
            TextColor: request.TextColor
        );
    }

    private static PointF GetCenteredOrigin(string text, Font font, Rectangle rect)
    {
        var bounds = TextMeasurer.MeasureBounds(text, new TextOptions(font)
        {
            WrappingLength = float.PositiveInfinity
        });

        var x = rect.X + (rect.Width - bounds.Width) / 2f - bounds.X;
        var y = rect.Y + (rect.Height - bounds.Height) / 2f - bounds.Y;

        return new PointF(x, y);
    }

}