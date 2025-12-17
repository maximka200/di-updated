using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class CloudPositionedTags(ICircularCloudLayouterWrapper cloudLayouter, ITagSizeCalculator tagSizeCalculator,
    float minFontSize, float maxFontSize)
    : ICloudPositionedTags
{
    public IEnumerable<PositionedTag> GetPositionedTags(IEnumerable<Tag> tags)
    {
        var tagList = tags.ToList();
        if (tagList.Count == 0)
            yield break;

        var minFreq = tagList.Min(t => t.Frequency);
        var maxFreq = tagList.Max(t => t.Frequency);
        
        foreach (var tag in tagList.OrderByDescending(t => t.Frequency))
        {
            var fontSize = GetFontSize(tag.Frequency, minFontSize, maxFontSize, minFreq, maxFreq);
            var size = tagSizeCalculator.GetSize(tag, fontSize);
            var rect = cloudLayouter.PutNextRectangle(size);
            yield return new PositionedTag(tag, rect, fontSize);
        }
    }

    private static int GetFontSize(int frequency, float minFontSize, float maxFontSize, int minFreq, int maxFreq)
    {
        var range = maxFreq - minFreq;
        var normalized = (frequency - minFreq) / range;
        return (int)(minFontSize + normalized * (maxFontSize - minFontSize));
    }
}