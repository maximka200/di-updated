using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public sealed class WordsSourceFactory
{
    public static IReadOnlyCollection<string> SourceFormats => sourceFormats;
    
    private static readonly string[] sourceFormats = Sources.Select(s => s.Format).ToArray();
    
    private static readonly IReadOnlyCollection<IWordsSource> Sources =
    [
        new TxtWordsSource()
    ];

    public IWordsSource Create(SourceSettings settings)
    {
        var source = Sources.FirstOrDefault(s => s.CanHandle(settings));
        if (source is null)
            throw new NotSupportedException($"Формат источника '{settings.Format}' не поддерживается");

        return source;
    }
}
