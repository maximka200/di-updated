using System.Globalization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Results;

namespace TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Strategies;

public class HexColorStrategy : IColorStrategy
{
    private static readonly IReadOnlyDictionary<int, Func<string, Color>> parsers =
        new Dictionary<int, Func<string, Color>>
        {
            [6] = ParseRgb,
            [8] = ParseArgb
        };

    public ColorResult TryParse(string raw)
    {
        try
        {
            var v = raw.Trim().TrimStart('#');
            return ColorResult.Resolved(parsers[v.Length](v));
        }
        catch
        {
            return ColorResult.Unresolved;
        }
    }

    private static Color ParseRgb(string v)
    {
        var r = ParseByte(v[..2]);
        var g = ParseByte(v[2..4]);
        var b = ParseByte(v[4..6]);
        return new Rgba32(r, g, b, 255);
    }

    private static Color ParseArgb(string v)
    {
        var a = ParseByte(v[..2]);
        var r = ParseByte(v[2..4]);
        var g = ParseByte(v[4..6]);
        var b = ParseByte(v[6..8]);
        return new Rgba32(r, g, b, a);
    }

    private static byte ParseByte(string hex) =>
        byte.Parse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
}