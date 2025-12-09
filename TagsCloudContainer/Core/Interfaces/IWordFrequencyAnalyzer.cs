namespace TagsCloudContainer.Core.Interfaces;

public interface IWordFrequencyAnalyzer
{
    IReadOnlyDictionary<string, int> GetFrequencies(IEnumerable<string> words);
}