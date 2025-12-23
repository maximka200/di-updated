using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.OutputFormats;

namespace TagsCloudContainer.Core.Interfaces;

public interface IImageSaver
{
    void Save(TagCloudGenerationRequest request, Image<Rgba32> image);
}