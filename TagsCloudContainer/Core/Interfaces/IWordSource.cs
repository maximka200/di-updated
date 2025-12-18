namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsSource
{
    string Format { get; }
    bool CanHandle(SourceSettings settings);
    IEnumerable<string> GetWords(string path);
}