using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core.OutputFormats;

public abstract class OutputSourceBase : IOutputFormat
{
    public abstract string Format { get; }

    public bool CanHandle(string format) =>
        string.Equals(Normalize(format), Format, StringComparison.OrdinalIgnoreCase);

    public void SaveImage(string path, Image image)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Output path is empty", nameof(path));

        SaveImageInternal(image, path);
    }

    protected abstract void SaveImageInternal(Image image, string path);

    private static string Normalize(string format) =>
        format.Trim().TrimStart('.').ToLowerInvariant();
}