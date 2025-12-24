using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.OutputFormats;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class OutputFormatSupport
{
    public static readonly List<IOutputFormat> Sources = [ 
        new PngOutputSource(), 
        new JpegOutputSource(),
        new JpegOutputSource() 
    ];
    
    public static void EnsureFormatSupported(string fmt)
    {
        var f = string.Concat(fmt).Trim().ToLowerInvariant();
        Ensure.True(Sources.Select(s => s.Format).Contains(f), $"Неподдерживаемый формат: {fmt}");
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