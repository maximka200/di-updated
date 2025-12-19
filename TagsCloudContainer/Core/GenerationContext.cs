using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public sealed class GenerationContext
{
    private TagCloudGenerationRequest Request { get; }
    private IEnumerable<string> Words { get; set; } = [];
    private IReadOnlyCollection<Tag> Tags { get; set; } = Array.Empty<Tag>();
    private IReadOnlyCollection<PositionedTag> PositionedTags { get; set; } = Array.Empty<PositionedTag>();
    private Image<Rgba32>? Image { get; set; }

    private GenerationContext(TagCloudGenerationRequest request) => Request = request;

    public static GenerationContext Start(TagCloudGenerationRequest request) =>
        new(request ?? throw new ArgumentNullException(nameof(request)));

    public GenerationContext ReadWords()
    {
        var source = WordsSourceFactory.Create(Request.SourceSettings);
        Words = source.GetWords(Request.SourceSettings.Path);
        return this;
    }

    public GenerationContext Preprocess(IWordsPreprocessor preprocessor)
    {
        Words = preprocessor.Process(Words);
        return this;
    }

    public GenerationContext BuildTags(IWordFrequencyAnalyzer analyzer)
    {
        var freq = analyzer.GetFrequencies(Words);
        Tags = freq.Select(x => new Tag(x.Key, x.Value)).ToList();
        
        return this;
    }

    public GenerationContext Layout(ICloudPositionedTags layouter)
    {
        PositionedTags = Tags.Count == 0
            ? Array.Empty<PositionedTag>()
            : layouter.GetPositionedTags(Tags, Request.LayoutSettings.MinFontSize, Request.LayoutSettings.MaxFontSize,
                Request.Desc).ToList();

        return this;
    }

    public GenerationContext Render()
    {
        CreateImage();

        if (PositionedTags.Count == 0)
            return this;

        var renderContext = BuildRenderContext();
        DrawAllTags(renderContext);

        return this;
    }

    private void CreateImage()
    {
        var size = Request.LayoutSettings.ImageSize;
        Image = new Image<Rgba32>(size.Width, size.Height, Request.BackgroundColor);
    }

    private RenderContext BuildRenderContext()
    {
        var settings = Request.LayoutSettings;

        var minFreq = PositionedTags.Min(p => p.Tag.Frequency);
        var maxFreq = PositionedTags.Max(p => p.Tag.Frequency);

        var fontFamily = FontFamilyResolver.Resolve(Request.Font);

        return new RenderContext(
            FontFamily: fontFamily,
            MinFontSize: settings.MinFontSize,
            MaxFontSize: settings.MaxFontSize,
            MinFreq: minFreq,
            MaxFreq: maxFreq,
            TextColor: Request.TextColor
        );
    }

    private void DrawAllTags(RenderContext ctx)
    {
        foreach (var (tag, rect, fontSize) in PositionedTags)
            DrawTag(ctx, tag, rect, fontSize);
    }

    private void DrawTag(RenderContext ctx, Tag tag, Rectangle rect, float fontSize)
    {
        var font = ctx.FontFamily.CreateFont(fontSize);

        var origin = GetCenteredOrigin(tag.Word, font, rect);

        var options = CreateTextOptions(font, origin);

        Image?.Mutate(i => i.DrawText(options, tag.Word, ctx.TextColor));
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
    
    private static RichTextOptions CreateTextOptions(Font font, PointF origin)
    {
        return new RichTextOptions(font)
        {
            Origin = origin,
            WrappingLength = float.PositiveInfinity
        };
    }

    public void Save()
    {
        if (Image is null)
            throw new InvalidOperationException("Ошибка генерации, изображение не сгенерировано");

        try
        {
            SaveImage(Request, Image);
        }
        finally
        {
            Image.Dispose();
        }
    }

    private static void SaveImage(TagCloudGenerationRequest request, Image<Rgba32> image)
    {
        var fmt = request.OutputFormat.ToLowerInvariant();

        var outputFormat = OutputFormatFactory.Create(request.OutputFormat, image);
        
        outputFormat.SaveImage(request.OutputPath);
    }
}