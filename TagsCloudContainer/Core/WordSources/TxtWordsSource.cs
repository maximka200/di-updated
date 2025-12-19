using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core.WordSources;

public class TxtWordsSource : IWordsSource
{
    public string Format => "txt";
    public bool CanHandle(SourceSettings settings) =>
        settings.Format.Equals(Format, StringComparison.InvariantCultureIgnoreCase);
    
    public IEnumerable<string> GetWords(string path) =>
        File.ReadLines(path).Where(line => !string.IsNullOrWhiteSpace(line));
}
