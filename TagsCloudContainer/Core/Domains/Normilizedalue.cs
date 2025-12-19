namespace TagsCloudContainer.Core.Domains;

public readonly struct NormalizedValue(float value)
{
    public static readonly NormalizedValue EqualFrequencies = new(float.NaN);

    public float OrElse(float avg)
    {
        var isNan = float.IsNaN(value);

        var f = value;
        return new Dictionary<bool, Func<float>>
        {
            [true] = () => avg,
            [false] = () => f
        }[isNan]();
    }
}