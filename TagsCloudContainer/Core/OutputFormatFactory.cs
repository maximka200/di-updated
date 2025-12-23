using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public static class OutputFormatFactory
{
    public static IOutputFormat Create(string format, IOutputFormat[] sources)
    {
        var normalized = Normalize(format);

        var source = sources.FirstOrDefault(d =>
            string.Equals(d.Format, normalized, StringComparison.OrdinalIgnoreCase));

        if (source is null)
            throw new NotSupportedException($"Формат вывода '{format}' не поддерживается");

        return source;
    }

    private static string Normalize(string format) =>
        format.Trim().TrimStart('.').ToLowerInvariant();
}