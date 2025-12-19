using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using SixLabors.Fonts;

namespace TagsCloudContainer.Core;

public class TagCloudGenerator(IWordsPreprocessor wordsPreprocessor,
    IWordFrequencyAnalyzer frequencyAnalyzer, ICloudPositionedTags cloudLayouter)
    : ITagCloudGenerator
{
    public void Generate(TagCloudGenerationRequest request)
    {
        GenerationContext.Start(request)
            .ReadWords()
            .Preprocess(wordsPreprocessor)
            .BuildTags(frequencyAnalyzer)
            .Layout(cloudLayouter)
            .Render()
            .Save();
    }
}
