using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class CloudPositionedTags(ICircularCloudLayouterWrapper cloudLayouter, ITagSizeCalculator tagSizeCalculator)
    : ICloudPositionedTags
{
    public IEnumerable<PositionedTag> GetPositionedTags(IEnumerable<Tag> tags, float minFontSize, float maxFontSize)
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

    private static float GetFontSize(int frequency, float minFontSize, float maxFontSize, int minFreq, int maxFreq)
    {
        if (minFontSize <= 0 || maxFontSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(minFontSize), "Font sizes must be > 0");

        if (minFontSize > maxFontSize)
            (minFontSize, maxFontSize) = (maxFontSize, minFontSize);

        if (minFreq == maxFreq)
            return (minFontSize + maxFontSize) / 2f;

        var normalized = (frequency - minFreq) / (float)(maxFreq - minFreq);
        normalized = Math.Clamp(normalized, 0f, 1f);

        return minFontSize + normalized * (maxFontSize - minFontSize);
    }
}