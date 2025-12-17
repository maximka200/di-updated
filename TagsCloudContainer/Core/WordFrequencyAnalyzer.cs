using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class WordFrequencyAnalyzer : IWordFrequencyAnalyzer
{
    public IReadOnlyDictionary<string, int> GetFrequencies(IEnumerable<string> words)
    {
        var dict = new Dictionary<string, int>();
        words = ToLowerCase(words);
        foreach (var word in words)
        {
            dict[word] = dict.GetValueOrDefault(word) + 1;
        }
        return dict;
    }

    private static IEnumerable<string> ToLowerCase(IEnumerable<string> words)
    {
        return words.Select(word => word.ToLower());
    }
}