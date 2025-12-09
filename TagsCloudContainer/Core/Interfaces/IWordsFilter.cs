namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsFilter
{
    bool ShouldKeep(string word);
}