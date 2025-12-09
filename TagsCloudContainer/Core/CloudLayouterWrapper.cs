using System.Drawing;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class CloudLayouterWrapper : ICloudLayouterWrapper 
{

    private ICircularCloudLayouter cloudLayouter;
    
    private readonly Func<Tag, Size> getTagSize = tag => new Size(tag.Frequency, 10);

    public IEnumerable<PositionedTag> GetPositionedTags(IEnumerable<Tag> tags)
    {
        return from tag
            in tags let
            rectangle = cloudLayouter.PutNextRectangle(getTagSize(tag))
            select new PositionedTag(tag, rectangle);
    }
}