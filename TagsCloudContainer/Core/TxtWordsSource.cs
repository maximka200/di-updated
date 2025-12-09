using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class TxtWordsSource(string path) : IWordsSource
{
    public IEnumerable<string> GetWords() =>
        File.ReadLines(path).Where(line => !string.IsNullOrWhiteSpace(line));
}
