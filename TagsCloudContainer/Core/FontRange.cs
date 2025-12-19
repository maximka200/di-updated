namespace TagsCloudContainer.Core;

public static class FontRange
{
    public static (float Min, float Max) Normalize(float minFontSize, float maxFontSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minFontSize);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxFontSize);

        var min = Math.Min(minFontSize, maxFontSize);
        var max = Math.Max(minFontSize, maxFontSize);
        return (min, max);
    }
}