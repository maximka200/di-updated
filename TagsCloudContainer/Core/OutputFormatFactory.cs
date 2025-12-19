using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.OutputFormats;

namespace TagsCloudContainer.Core;

public static class OutputFormatFactory
{
    public static IReadOnlyCollection<string> OutputFormats => outputFormats;

    private static readonly OutputDescriptor[] descriptors =
    [
        new("png", img => new PngOutputSource(img)),
        new("jpg", img => new JpegOutputSource(img)),
        new("jpeg", img => new JpegOutputSource(img)),
    ];

    private static readonly string[] outputFormats =
        descriptors.Select(d => d.Format).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

    public static IOutputFormat Create(string format, Image image)
    {
        ArgumentNullException.ThrowIfNull(image);

        var normalized = Normalize(format);

        var descriptor = descriptors.FirstOrDefault(d =>
            string.Equals(d.Format, normalized, StringComparison.OrdinalIgnoreCase));

        if (descriptor is null)
            throw new NotSupportedException($"Формат вывода '{format}' не поддерживается");

        return descriptor.Factory(image);
    }

    private static string Normalize(string format) =>
        format.Trim().TrimStart('.').ToLowerInvariant();

    private sealed record OutputDescriptor(string Format, Func<Image, IOutputFormat> Factory);
}