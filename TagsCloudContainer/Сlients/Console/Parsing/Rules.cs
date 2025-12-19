using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal class PositiveIntRule(string label) : IIntRule
{
    public string Label => label;

    private static readonly IReadOnlyDictionary<bool, Func<int, int>> Map =
        new Dictionary<bool, Func<int, int>>
        {
            [true] = v => v,
            [false] = _ => throw new Exception("invalid")
        };

    public int Validate(int value)
    {
        try { return Map[value > 0](value); }
        catch (Exception) { throw new Exception($"Некорректный {label}: {value}"); }
    }
}

internal class NonNegativeIntRule(string label) : IIntRule
{
    public string Label => label;

    private static readonly IReadOnlyDictionary<bool, Func<int, int>> Map =
        new Dictionary<bool, Func<int, int>>
        {
            [true] = v => v,
            [false] = _ => throw new Exception("invalid")
        };

    public int Validate(int value)
    {
        try { return Map[value >= 0](value); }
        catch (Exception) { throw new Exception($"Некорректный {label}: {value}"); }
    }
}

internal class PositiveFloatRule(string label) : IFloatRule
{
    public string Label => label;

    private static readonly IReadOnlyDictionary<bool, Func<float, float>> Map =
        new Dictionary<bool, Func<float, float>>
        {
            [true] = v => v,
            [false] = _ => throw new Exception("invalid")
        };

    public float Validate(float value)
    {
        try { return Map[value > 0](value); }
        catch (Exception) { throw new Exception($"Некорректный {label}: {value}"); }
    }
}