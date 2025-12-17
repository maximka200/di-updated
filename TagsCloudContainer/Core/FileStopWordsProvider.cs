using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class FileStopWordsProvider(IWordNormalizer normalizer, string? stopWordsPath) : IStopWordsProvider
{
    private Lazy<ISet<string>> LazyStopWords => new(LoadStopWords);
    
    public ISet<string> GetStopWords() => LazyStopWords.Value;

    private ISet<string> LoadStopWords()
    {
        if (string.IsNullOrEmpty(stopWordsPath) || !File.Exists(stopWordsPath))
            throw new NullReferenceException($"Ошибка чтения: {stopWordsPath}");

        return File
            .ReadAllLines(stopWordsPath)
            .Select(normalizer.Normalize)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet();
    }
}