using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
namespace TagsCloudContainer.Core;

public class TagCloudGenerator(
    IWordsReader wordsReader,
    IWordsPreprocessor wordsPreprocessor,
    ITagsBuilder tagsBuilder,
    ILayoutService layoutService,
    ICloudRenderer renderer,
    IImageSaver saver)
    : ITagCloudGenerator
{
    public void Generate(TagCloudGenerationRequest request)
    {
        GenerationContext.Start(request)
            .ReadWords(wordsReader)
            .Preprocess(wordsPreprocessor)
            .BuildTags(tagsBuilder)
            .Layout(layoutService)
            .Render(renderer)
            .Save(saver);
    }
}
