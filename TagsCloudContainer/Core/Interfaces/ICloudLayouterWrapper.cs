using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface ICloudLayouterWrapper 
{
    IEnumerable<PositionedTag> GetPositionedTags(IEnumerable<Tag> tags);
}