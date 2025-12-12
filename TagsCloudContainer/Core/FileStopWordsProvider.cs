using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class FileStopWordsProvider : IStopWordsProvider
{
    private readonly string? stopWordsPath;
    private readonly Lazy<ISet<string>> lazyStopWords;
    private readonly IWordNormalizer wordNormalizer;

    public FileStopWordsProvider(IWordNormalizer normalizer, string? stopWordsPath = null)
    {
        this.stopWordsPath = stopWordsPath;
        lazyStopWords = new Lazy<ISet<string>>(LoadStopWords);
        wordNormalizer = normalizer;
    }

    private ISet<string> LoadStopWords()
    {
        if (string.IsNullOrEmpty(stopWordsPath) || !File.Exists(stopWordsPath))
            return new HashSet<string>(); 

        return File
            .ReadAllLines(stopWordsPath)
            .Select(x => wordNormalizer.Normalize(x))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet();
    }

    public ISet<string> GetStopWords() => lazyStopWords.Value;
}