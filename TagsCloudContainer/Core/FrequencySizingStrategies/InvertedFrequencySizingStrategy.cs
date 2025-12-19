using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core.FrequencySizingStrategies;

public class InvertedFrequencySizingStrategy : IFrequencySizingStrategy
{
    public bool Inverted => true;

    public IEnumerable<Tag> Order(IEnumerable<Tag> tags) =>
        tags.OrderBy(t => t.Frequency); 

    public float Scale(float minFontSize, float maxFontSize, float normalized)
    {
        return minFontSize + normalized * (maxFontSize - minFontSize);
    }
}