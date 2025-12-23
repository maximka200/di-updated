using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface ICloudRenderer
{
    Image<Rgba32> Render(TagCloudGenerationRequest request, IReadOnlyCollection<PositionedTag> positionedTags);
}