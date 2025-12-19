using TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Results;
using TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Strategies;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console.Parsing.ColorParsing;

internal static class ColorParser
{
    private static readonly IColorStrategy[] Strategies =
    [
        new NamedColorStrategy(),
        new HexColorStrategy()
    ];

    public static Color Parse(string key, string raw)
    {
        var r = Strategies
            .Select(s => s.TryParse(raw))
            .Aggregate(ColorResult.Unresolved, (acc, next) => acc.OrElse(() => next));

        return r.Unwrap(key, raw);
    }
}
