using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace TagsCloudContainer.Core.OutputFormats;

public sealed class PngOutputSource: OutputSourceBase
{
    public override string Format => "png";

    protected override void SaveImageInternal(Image image, string path) =>
        image.Save(path, new PngEncoder());
}