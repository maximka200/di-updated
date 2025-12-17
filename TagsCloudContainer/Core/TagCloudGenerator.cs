using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using SixLabors.Fonts;

namespace TagsCloudContainer.Core;

public class TagCloudGenerator(IWordsSourceFactory wordsSourceFactory, IWordsPreprocessor wordsPreprocessor,
    IWordFrequencyAnalyzer frequencyAnalyzer, ICloudPositionedTags cloudLayouter, FontFamily? fontFamily = null)
    : ITagCloudGenerator
{
    private readonly FontFamily fontFamily = fontFamily ?? SystemFonts.Collection.Families.First();

    public void Generate(TagCloudGenerationRequest request)
    {
        GenerationContext.Start(request)
            .ReadWords(wordsSourceFactory)
            .Preprocess(wordsPreprocessor)
            .BuildTags(frequencyAnalyzer)
            .Layout(cloudLayouter)
            .Render(fontFamily)
            .Save();
    }
}
