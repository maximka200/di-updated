namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsSource
{
    bool CanHandle(SourceSettings settings);
    IEnumerable<string> GetWords(string path);
}