using System.Globalization;
using TagsCloudContainer.Сlients.Console.Parsing.ColorParsing;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using TagsCloudContainer.Сlients.Exceptions;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class Ensure
{
    private static readonly IReadOnlyDictionary<bool, Action<string>> RequireTrue =
        new Dictionary<bool, Action<string>>
        {
            [true] = _ => { },
            [false] = m => throw new ConsoleParsingException(m)
        };

    private static readonly IReadOnlyDictionary<bool, Action<string>> RequireFlagPrefix =
        new Dictionary<bool, Action<string>>
        {
            [true] = _ => { },
            [false] = a => throw new ConsoleParsingException($"Ожидался флаг, но получено: '{a}'")
        };

    private static readonly IReadOnlyDictionary<bool, Action<(string Next, string Key)>> nextTokenIsValue =
        new Dictionary<bool, Action<(string Next, string Key)>>
        {
            [false] = _ => { },
            [true] = p => throw new ConsoleParsingException($"Ожидалось значение после {p.Key}")
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

    public static int ParseIntOrDefault(string? value, int def, IIntRule rule) =>
        ParseOrDefault(
            value, def,
            parse: s => int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture),
            validate: rule.Validate,
            errorMessage: () => $"Некорректный {rule.Label}: {value}"
        );

    public static float ParseFloatOrDefault(string? value, float def, IFloatRule rule) =>
        ParseOrDefault(
            value, def,
            parse: s => float.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture),
            validate: rule.Validate,
            errorMessage: () => $"Некорректный {rule.Label}: {value}"
        );

    public static Color ParseColorOrDefault(string key, string? value, Color def) =>
        ParseOrDefault(
            value, def,
            parse: s => ColorParser.Parse(key, s),
            validate: c => c, // без доп. валидации
            errorMessage: () => $"Некорректный {key}: {value}"
        );

    public static bool ParseBoolOrDefault(string? value, bool def, string label) =>
        ParseOrDefault(
            value, def,
            parse: s => BoolParser.Parse(s, label),
            validate: b => b,
            errorMessage: () => $"Некорректный {label}: {value}"
        );

    private static T ParseOrDefault<T>(string? value, T def,
        Func<string, T> parse, Func<T, T> validate,
        Func<string> errorMessage)
    {
        var s = string.Concat(value).Trim();

        if (string.IsNullOrWhiteSpace(s))
            return def;

        try
        {
            var parsed = parse(s);
            return validate(parsed);
        }
        catch (ConsoleParsingException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new ConsoleParsingException(errorMessage());
        }
    }
}

