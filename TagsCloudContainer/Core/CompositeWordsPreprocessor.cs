using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class CompositeWordsPreprocessor(IWordNormalizer normalizer,
    IEnumerable<IWordsFilter> filters) : IWordsPreprocessor
{
    public IEnumerable<string> Process(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            var normalized = normalizer.Normalize(word);
            if (string.IsNullOrWhiteSpace(normalized))
                continue;

            var keep = true;
            foreach (var filter in filters)
                if (!filter.ShouldKeep(normalized))
                    keep = false;

            if (keep)
                yield return normalized;
        }
    }
}
