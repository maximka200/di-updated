namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class BoolParser
{
    private static readonly IReadOnlyDictionary<string, Func<bool>> Map =
        new Dictionary<string, Func<bool>>(StringComparer.OrdinalIgnoreCase)
        {
            ["true"] = static () => true,
            ["1"] = static () => true,
            ["yes"] = static () => true,
            ["on"] = static () => true,

            ["false"] = static () => false,
            ["0"] = static () => false,
            ["no"] = static () => false,
            ["off"] = static () => false
        };

    public static bool Parse(string raw, string label)
    {
        try
        {
            return Map[raw.Trim()]();
        }
        catch (KeyNotFoundException)
        {
            throw new Exception($"Некорректный {label}: {raw}");
        }
    }
}