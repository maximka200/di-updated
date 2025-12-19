using System.Text.RegularExpressions;

namespace TagsCloudContainer.Core.WordSources;

internal static partial class WordTokenizer
{
    private const string Pattern = @"[\p{L}\p{Nd}]+";
    
    [GeneratedRegex(Pattern, RegexOptions.CultureInvariant)]
    private static partial Regex MyRegex();

    public static IEnumerable<string> Tokenize(string? text) =>
        MyRegex().Matches(text ?? string.Empty)
            .Select(m => m.Value)
            .Where(s => !string.IsNullOrWhiteSpace(s));
}