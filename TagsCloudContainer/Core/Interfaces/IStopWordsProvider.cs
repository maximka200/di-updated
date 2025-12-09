namespace TagsCloudContainer.Core.Interfaces;

public interface IStopWordsProvider
{
    ISet<string> GetStopWords();
}