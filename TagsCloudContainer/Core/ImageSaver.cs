using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class ImageSaver(IOutputFormat[] sources): IImageSaver
{
    public void Save(TagCloudGenerationRequest request, Image<Rgba32> image)
    {
        var source = OutputFormatFactory.Create(request.OutputFormat, sources);
        source.SaveImage(request.OutputPath, image);
    }
}
