using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.WordSources;

namespace TagsCloudContainer.Core;

public static class WordsSourceFactory
{
    public static IWordsSource Create(SourceSettings settings, IWordsSource[] sources)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var format = settings.Format.Trim();

        var source = sources.FirstOrDefault(s =>
            s.CanHandle(new SourceSettings(settings.Path, format)));

        if (source is null)
            throw new NotSupportedException($"Формат источника '{settings.Format}' не поддерживается");

        return source;
    }
}