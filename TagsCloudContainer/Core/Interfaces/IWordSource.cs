namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsSource
{
    IEnumerable<string> GetWords();
}