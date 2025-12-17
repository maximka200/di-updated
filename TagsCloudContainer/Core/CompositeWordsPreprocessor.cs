using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class CompositeWordsPreprocessor(IWordNormalizer normalizer,
    IEnumerable<IWordsFilter> filters) : IWordsPreprocessor
{
    public IEnumerable<string> Process(IEnumerable<string> words) =>
        words
            .Select(normalizer.Normalize)
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Where(w => filters.All(f => f.ShouldKeep(w)));
}
