using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.WordSources;

namespace TagsCloudContainer.Core;

public static class WordsSourceFactory
{
    private static readonly IWordsSource[] Sources =
    [
        new TxtWordsSource(),
    ];

    private static readonly string[] sourceFormats =
        Sources.Select(s => s.Format)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    public static IReadOnlyCollection<string> SourceFormats => sourceFormats;

    public static IWordsSource Create(SourceSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var format = (settings.Format ?? string.Empty).Trim();

        var source = Sources.FirstOrDefault(s =>
            s.CanHandle(new SourceSettings(settings.Path, format)));

        if (source is null)
            throw new NotSupportedException($"Формат источника '{settings.Format}' не поддерживается");

        return source;
    }
}