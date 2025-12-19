
using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface IFrequencySizingStrategy
{
    bool Inverted { get; }
    IEnumerable<Tag> Order(IEnumerable<Tag> tags);
    float Scale(float minFontSize, float maxFontSize, float normalized01);
}