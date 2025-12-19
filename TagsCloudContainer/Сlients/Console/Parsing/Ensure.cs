using System.Globalization;
using TagsCloudContainer.Сlients.Console.Parsing.ColorParsing;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class Ensure
{
    private static readonly IReadOnlyDictionary<bool, Action<string>> RequireTrue =
        new Dictionary<bool, Action<string>>
        {
            [true] = _ => { },
            [false] = m => throw new Exception(m)
        };

    private static readonly IReadOnlyDictionary<bool, Action<string>> RequireFlagPrefix =
        new Dictionary<bool, Action<string>>
        {
            [true] = _ => { },
            [false] = a => throw new Exception($"Ожидался флаг, но получено: '{a}'")
        };

    private static readonly IReadOnlyDictionary<bool, Action<(string Next, string Key)>> nextTokenIsValue =
        new Dictionary<bool, Action<(string Next, string Key)>>
        {
            [false] = _ => { },
            [true] = p => throw new Exception($"Ожидалось значение после {p.Key}")
        };

    public static void True(bool condition, string message) => RequireTrue[condition](message);

    public static void StartsWithFlagPrefix(string arg) =>
        RequireFlagPrefix[arg.StartsWith("--", StringComparison.Ordinal)](arg);

    public static void NextTokenIsValue(string next, string key) =>
        nextTokenIsValue[next.StartsWith("--", StringComparison.Ordinal)]((next, key));

    public static void FileExists(string path, string message) =>
        RequireTrue[File.Exists(path)](message);

    public static string TrimmedNonEmpty(string? value, string message)
    {
        var s = string.Concat(value).Trim();
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(s);
            return s;
        }
        catch (ArgumentException)
        {
            throw new Exception(message);
        }
    }

    public static string TrimmedOrDefault(string? value, string def)
    {
        var s = string.Concat(value).Trim();
        return new Dictionary<bool, Func<string>>
        {
            [true] = () => s,
            [false] = () => def
        }[!string.IsNullOrWhiteSpace(s)]();
    }

    public static int ParseIntOrDefault(string? value, int def, IIntRule rule)
    {
        var s = string.Concat(value).Trim();

        return new Dictionary<bool, Func<int>>
        {
            [false] = () => def,
            [true] = () =>
            {
                try
                {
                    var n = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    return rule.Validate(n);
                }
                catch (Exception)
                {
                    throw new Exception($"Некорректный {rule.Label}: {value}");
                }
            }
        }[!string.IsNullOrWhiteSpace(s)]();
    }

    public static float ParseFloatOrDefault(string? value, float def, IFloatRule rule)
    {
        var s = string.Concat(value).Trim();

        return new Dictionary<bool, Func<float>>
        {
            [false] = () => def,
            [true] = () =>
            {
                try
                {
                    var f = float.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture);
                    return rule.Validate(f);
                }
                catch (Exception)
                {
                    throw new Exception($"Некорректный {rule.Label}: {value}");
                }
            }
        }[!string.IsNullOrWhiteSpace(s)]();
    }

    public static Color ParseColorOrDefault(string key, string? value, Color def)
    {
        var s = string.Concat(value).Trim();

        return new Dictionary<bool, Func<Color>>
        {
            [false] = () => def,
            [true] = () => ColorParser.Parse(key, s)
        }[!string.IsNullOrWhiteSpace(s)]();
    }
    
    public static bool ParseBoolOrDefault(string? value, bool def, string label)
    {
        var s = string.Concat(value).Trim();

        return new Dictionary<bool, Func<bool>>
        {
            [false] = () => def,
            [true] = () => BoolParser.Parse(s, label)
        }[!string.IsNullOrWhiteSpace(s)]();
    }
}
