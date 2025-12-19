using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core.FrequencySizingStrategies;

public class DirectFrequencySizingStrategy : IFrequencySizingStrategy
{
    public bool Inverted => false;

    public IEnumerable<Tag> Order(IEnumerable<Tag> tags) =>
        tags.OrderByDescending(t => t.Frequency);

    public float Scale(float minFontSize, float maxFontSize, float normalized) =>
        minFontSize + normalized * (maxFontSize - minFontSize);
}