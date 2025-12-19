using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Ranges;

namespace TagsCloudContainer.Core;

public static class FrequencyRange
{
    public static MaybeRange TryGet(IReadOnlyList<Tag> tags)
    {
        try
        {
            var min = tags.Min(t => t.Frequency);
            var max = tags.Max(t => t.Frequency);
            return MaybeRange.Range(min, max);
        }
        catch (InvalidOperationException)
        {
            return MaybeRange.Empty;
        }
    }
}