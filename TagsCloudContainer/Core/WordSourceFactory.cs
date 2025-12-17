using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public sealed class WordsSourceFactory : IWordsSourceFactory
{
    private readonly IReadOnlyCollection<IWordsSource> sources =
    [
        new TxtWordsSource()
    ];

    public IWordsSource Create(SourceSettings settings)
    {
        var source = sources.FirstOrDefault(s => s.CanHandle(settings));
        if (source is null)
            throw new NotSupportedException($"Формат источника '{settings.Format}' не поддерживается");

        return source;
    }
}
