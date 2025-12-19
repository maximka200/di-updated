using NPOI.XWPF.UserModel;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core.WordSources;

public sealed class DocxWordsSource : IWordsSource
{
    public string Format => "docx";

    public bool CanHandle(SourceSettings settings) =>
        settings.Format.Equals(Format, StringComparison.InvariantCultureIgnoreCase);

    public IEnumerable<string> GetWords(string path)
    {
        using var fs = File.OpenRead(path);
        var doc = new XWPFDocument(fs);

        var text = string.Join(
            Environment.NewLine,
            doc.Paragraphs.Select(p => p.Text));

        return WordTokenizer.Tokenize(text);
    }
}