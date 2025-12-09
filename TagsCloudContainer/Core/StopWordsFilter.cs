using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class StopWordsFilter(IStopWordsProvider stopWordsProvider) : IWordsFilter
{
    public bool ShouldKeep(string word) =>
        !stopWordsProvider.GetStopWords().Contains(word);
}