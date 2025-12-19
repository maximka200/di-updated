using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace TagsCloudContainer.Core.OutputFormats;


public sealed class JpegOutputSource(Image image) : OutputSourceBase(image)
{
    public override string Format => "jpg";

    protected override void SaveImageInternal(Image image, string path) =>
        image.Save(path, new JpegEncoder { Quality = 90 });
}
