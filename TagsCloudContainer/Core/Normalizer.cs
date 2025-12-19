using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core;

public static class Normalizer
{
    public static NormalizedValue Normalize(int frequency, int minFreq, int maxFreq)
    {
        try
        {
            var denom = (float)(maxFreq - minFreq);
            var value = (frequency - minFreq) / denom;
            return new NormalizedValue(Math.Clamp(value, 0f, 1f));
        }
        catch (DivideByZeroException)
        {
            return NormalizedValue.EqualFrequencies;
        }
    }
}