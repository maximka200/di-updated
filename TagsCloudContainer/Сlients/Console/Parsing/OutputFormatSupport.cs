using TagsCloudContainer.Core;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class OutputFormatSupport
{
    public static void EnsureFormatSupported(string fmt)
    {
        var f = string.Concat(fmt).Trim().ToLowerInvariant();
        Ensure.True(OutputFormatFactory.OutputFormats.Contains(f), $"Неподдерживаемый формат: {fmt}");
    }

    public static string FormatFromPath(string outputPath)
    {
        var ext = Path.GetExtension(outputPath);

        return new Dictionary<bool, Func<string>>
        {
            [true] = () => "png",
            [false] = () => ext.TrimStart('.').ToLowerInvariant()
        }[string.IsNullOrWhiteSpace(ext)]();
    }
}