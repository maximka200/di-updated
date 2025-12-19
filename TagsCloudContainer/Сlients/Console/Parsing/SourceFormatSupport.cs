using TagsCloudContainer.Core;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class SourceFormatSupport
{
    public static void EnsureFormatSupported(string fmt)
    {
        var f = string.Concat(fmt).Trim().ToLowerInvariant();
        Ensure.True(WordsSourceFactory.WordSourceFormats.Contains(f), $"Неподдерживаемый формат источника: {fmt}");
    }

    public static string FormatFromPath(string inputPath)
    {
        var ext = Path.GetExtension(inputPath);

        return new Dictionary<bool, Func<string>>
        {
            [true] = () => "txt",
            [false] = () => ext.TrimStart('.').ToLowerInvariant()
        }[string.IsNullOrWhiteSpace(ext)]();
    }
}