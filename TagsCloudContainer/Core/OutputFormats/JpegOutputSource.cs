using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace TagsCloudContainer.Core.OutputFormats;


public sealed class JpegOutputSource : OutputSourceBase
{
    public override string Format => "jpg";

    private const int Quality = 90;

    protected override void SaveImageInternal(Image image, string path) =>
        image.Save(path, new JpegEncoder { Quality = Quality });
}
