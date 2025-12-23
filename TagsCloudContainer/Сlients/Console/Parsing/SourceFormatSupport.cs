using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.WordSources;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class SourceFormatSupport
{
    public static readonly List<IWordsSource> Sources =
    [
        new TxtWordsSource(),
        new DocWordsSource(),
        new DocxWordsSource()
    ];
    
    public static void EnsureFormatSupported(string src)
    {
        var f = string.Concat(src).Trim().ToLowerInvariant();
        Ensure.True(Sources.Select(s => s.Format).Contains(src), $"Неподдерживаемый формат источника: {src}");
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