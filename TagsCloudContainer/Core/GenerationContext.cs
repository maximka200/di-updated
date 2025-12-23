using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.OutputFormats;

namespace TagsCloudContainer.Core;

public class GenerationContext
{
    private readonly TagCloudGenerationRequest request;

    private IEnumerable<string> words = Array.Empty<string>();
    private IReadOnlyCollection<Tag> tags = Array.Empty<Tag>();
    private IReadOnlyCollection<PositionedTag> positionedTags = Array.Empty<PositionedTag>();
    private Image<Rgba32>? image;

    private GenerationContext(TagCloudGenerationRequest request) =>
        this.request = request ?? throw new ArgumentNullException(nameof(request));

    public static GenerationContext Start(TagCloudGenerationRequest request) => new(request);

    public GenerationContext ReadWords(IWordsReader reader)
    {
        words = reader.Read(request);
        return this;
    }

    public GenerationContext Preprocess(IWordsPreprocessor preprocessor)
    {
        words = preprocessor.Process(words);
        return this;
    }

    public GenerationContext BuildTags(ITagsBuilder builder)
    {
        tags = builder.Build(words);
        return this;
    }

    public GenerationContext Layout(ILayoutService layout)
    {
        positionedTags = tags.Count == 0 ? Array.Empty<PositionedTag>() : layout.Layout(request, tags);
        return this;
    }

    public GenerationContext Render(ICloudRenderer renderer)
    {
        image = renderer.Render(request, positionedTags);
        return this;
    }

    public void Save(IImageSaver saver)
    {
        if (image is null)
            throw new InvalidOperationException("Ошибка генерации, изображение не сгенерировано");

        try
        {
            saver.Save(request, image);
        }
        finally
        {
            image.Dispose();
            image = null;
        }
    }
}
