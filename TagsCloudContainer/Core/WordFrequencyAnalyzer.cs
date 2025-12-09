using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class WordFrequencyAnalyzer : IWordFrequencyAnalyzer
{
    public IReadOnlyDictionary<string, int> GetFrequencies(IEnumerable<string> words)
    {
        var dict = new Dictionary<string, int>();
        foreach (var word in words)
        {
            dict.TryGetValue(word, out var count);
            dict[word] = count + 1;
        }
        return dict;
    }
}