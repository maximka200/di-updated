using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class TxtWordsSource : IWordsSource
{
    public bool CanHandle(SourceSettings settings) =>
        settings.Format.Equals("txt", StringComparison.InvariantCultureIgnoreCase);
    
    public IEnumerable<string> GetWords(string path) =>
        File.ReadLines(path).Where(line => !string.IsNullOrWhiteSpace(line));
}
