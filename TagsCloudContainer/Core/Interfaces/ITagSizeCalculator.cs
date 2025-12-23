using SixLabors.Fonts;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface ITagSizeCalculator
{
    Size GetSize(Tag tag, float fontSize, FontFamily fontFamily);
}