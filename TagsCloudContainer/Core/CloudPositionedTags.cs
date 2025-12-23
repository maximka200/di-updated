using SixLabors.Fonts;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.FrequencySizingStrategies;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class CloudPositionedTags(
    ICircularCloudLayouterWrapper cloudLayouter,
    ITagSizeCalculator tagSizeCalculator)
    : ICloudPositionedTags
{
    private static readonly IReadOnlyDictionary<bool, IFrequencySizingStrategy> Strategies =
        new IFrequencySizingStrategy[]
        {
            new DirectFrequencySizingStrategy(),
            new InvertedFrequencySizingStrategy()
        }.ToDictionary(s => s.Inverted);

    public IEnumerable<PositionedTag> GetPositionedTags(IEnumerable<Tag> tags, float minFontSize,
        float maxFontSize, bool invertSizeByFrequency, FontFamily ff)
    {
        ArgumentNullException.ThrowIfNull(tags);

        var tagList = tags.ToList();

        var (minFreq, maxFreq) = FrequencyRange.TryGet(tagList).GetOrYieldBreak();

        var (minFont, maxFont) = FontRange.Normalize(minFontSize, maxFontSize);

        var strategy = Strategies[invertSizeByFrequency];

        foreach (var tag in strategy.Order(tagList))
        {
            var fontSize = GetFontSize(tag.Frequency, minFont, maxFont, minFreq, maxFreq, strategy);
            var size = tagSizeCalculator.GetSize(tag, fontSize, ff);
            var rect = cloudLayouter.PutNextRectangle(size);
            yield return new PositionedTag(tag, rect, fontSize);
        }
    }

    private static float GetFontSize(int frequency, float minFontSize,
        float maxFontSize, int minFreq, int maxFreq,
        IFrequencySizingStrategy strategy)
    {
        var avg = (minFontSize + maxFontSize) / 2f;

        var normalized = Normalizer.Normalize(frequency, minFreq, maxFreq).OrElse(avg);
        return strategy.Scale(minFontSize, maxFontSize, normalized);
    }
}
