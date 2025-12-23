using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Interfaces;

public interface IOutputFormat
{
    string Format { get; }
    bool CanHandle(string format);
    void SaveImage(string path, Image image);
}