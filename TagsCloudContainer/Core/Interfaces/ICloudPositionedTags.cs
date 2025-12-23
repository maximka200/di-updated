using SixLabors.Fonts;
using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface ICloudPositionedTags 
{
    IEnumerable<PositionedTag> GetPositionedTags(IEnumerable<Tag> tags, float minFontSize, float maxFontSize,
        bool invertSizeByFrequency, FontFamily ff);
}