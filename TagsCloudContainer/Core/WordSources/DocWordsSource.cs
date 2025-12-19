using NPOI.HWPF;
using NPOI.HWPF.Extractor;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core.WordSources;

public sealed class DocWordsSource : IWordsSource
{
    public string Format => "doc";

    public bool CanHandle(SourceSettings settings) =>
        settings.Format.Equals(Format, StringComparison.InvariantCultureIgnoreCase);

    public IEnumerable<string> GetWords(string path)
    {
        using var fs = File.OpenRead(path);
        var doc = new HWPFDocument(fs);
        var extractor = new WordExtractor(doc);

        var text = extractor.Text;
        return WordTokenizer.Tokenize(text);
    }
}