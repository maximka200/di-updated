using SixLabors.ImageSharp;
using TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Results;

namespace TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Strategies;

public class NamedColorStrategy : IColorStrategy
{
    private static readonly IReadOnlyDictionary<string, Func<Color>> Map =
        new Dictionary<string, Func<Color>>(StringComparer.OrdinalIgnoreCase)
        {
            ["white"] = () => Color.White,
            ["black"] = () => Color.Black
        };

    public ColorResult TryParse(string raw)
    {
        try { return ColorResult.Resolved(Map[raw.Trim()]()); }
        catch (KeyNotFoundException) { return ColorResult.Unresolved; }
    }
}