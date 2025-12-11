using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

using SixLabors.Fonts;

namespace TagsCloudContainer.Core;

public class TagCloudGenerator : ITagCloudGenerator
{
    private readonly IWordsSourceFactory wordsSourceFactory;
    private readonly IWordsPreprocessor wordsPreprocessor;
    private readonly IWordFrequencyAnalyzer frequencyAnalyzer;
    private readonly ICloudLayouterWrapper cloudLayouter;

    private FontFamily fontFamily;

    public TagCloudGenerator(IWordsSourceFactory wordsSourceFactory, IWordsPreprocessor wordsPreprocessor,
        IWordFrequencyAnalyzer frequencyAnalyzer, ICloudLayouterWrapper cloudLayouter)
    {
        this.wordsSourceFactory = wordsSourceFactory;
        this.wordsPreprocessor = wordsPreprocessor;
        this.frequencyAnalyzer = frequencyAnalyzer;
        this.cloudLayouter = cloudLayouter;
        
        if (!SystemFonts.TryGet("Arial", out var fam))
            fam = SystemFonts.Collection.Families.First();

        fontFamily = fam;
    }

    public void Generate(TagCloudGenerationRequest request)
    {
        var source = wordsSourceFactory.Create(request.SourceSettings);
        var words = source.GetWords();
        words = wordsPreprocessor.Process(words);
        
        var freq = frequencyAnalyzer.GetFrequencies(words);
        var tags = freq.Select(t => new Tag(t.Key, t.Value)).ToList();

        var imgSize = request.LayoutSettings.ImageSize;

        if (tags.Count == 0)
        {
            CreateEmptyImage(request, imgSize);
            return;
        }
        
        var positioned = cloudLayouter.GetPositionedTags(tags).ToList();

        RenderImage(request, positioned);
    }

    private void CreateEmptyImage(TagCloudGenerationRequest request, System.Drawing.Size size)
    {
        using Image<Rgba32> image = new(size.Width, size.Height);
        image.Mutate(ctx => ctx.Fill(ToColor(request.BackgroundColor)));
        SaveImage(request, image);
    }

    private void RenderImage(
        TagCloudGenerationRequest request,
        IReadOnlyCollection<PositionedTag> positionedTags)
    {
        var settings = request.LayoutSettings;
        var imgSize = settings.ImageSize;

        using Image<Rgba32> image = new(imgSize.Width, imgSize.Height);
        image.Mutate(ctx => ctx.Fill(ToColor(request.BackgroundColor)));

        var minFreq = positionedTags.Min(p => p.Tag.Frequency);
        var maxFreq = positionedTags.Max(p => p.Tag.Frequency);

        foreach (var positioned in positionedTags)
        {
            var tag = positioned.Tag;
            var rect = positioned.Rectangle;

            var fontSize = ScaleFont(
                tag.Frequency,
                settings.MinFontSize,
                settings.MaxFontSize,
                minFreq,
                maxFreq);

            var font = fontFamily.CreateFont(fontSize);
            
            var measureOptions = new TextOptions(font)
            {
                WrappingLength = float.PositiveInfinity
            };

            var measured = TextMeasurer.MeasureSize(tag.Word, measureOptions);
            var x = rect.X + (rect.Width - measured.Width) / 2f;
            var y = rect.Y + (rect.Height - measured.Height) / 2f;
            
            var richOptions = new RichTextOptions(font)
            {
                Origin = new PointF(x, y),
                WrappingLength = float.PositiveInfinity
            };

            var textColor = ToColor(request.TextColor);

            image.Mutate(ctx => ctx.DrawText(richOptions, tag.Word, textColor));
        }

        SaveImage(request, image);
    }
    private static Rgba32 ToColor(System.Drawing.Color c) => new(c.R, c.G, c.B, c.A);
    
    private static float ScaleFont(int frequency, float minSize,
        float maxSize, int minFreq, int maxFreq)
    {
        float range = maxFreq - minFreq + 1;
        var norm = (frequency - minFreq + 0.5f) / range;
        return minSize + norm * (maxSize - minSize);
    }

    private static void SaveImage(TagCloudGenerationRequest request,
        Image<Rgba32> image)
    {
        var fmt = request.OutputFormat.ToLowerInvariant();

        switch (fmt)
        {
            case "jpg" or "jpeg":
                image.SaveAsJpeg(request.OutputPath);
                break;
            case "bmp":
                image.SaveAsBmp(request.OutputPath);
                break;
            default:
                image.SaveAsPng(request.OutputPath);
                break;
        }
    }
}
