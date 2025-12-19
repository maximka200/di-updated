using SixLabors.Fonts;
using TagsCloudContainer.Core.FontStrategies;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public static class FontFamilyResolver
{
    private static readonly IFontChoiceStrategy[] Strategies =
    [
        new ArialFontStrategy(),
        new HelveticaFontStrategy(),
        new MenloFontStrategy()
    ];

    private static readonly IReadOnlyDictionary<string, IFontChoiceStrategy> Map = CreateMap(Strategies);

    public static IReadOnlyCollection<string?> Choices =>
        Strategies.Select(s => s.Key).ToArray();

    public static FontFamily Resolve(string? choice)
    {
        var key = Normalize(choice);

        try { return Map[key].Resolve(); }
        catch (KeyNotFoundException)
        {
            throw new NotSupportedException(
                $"Шрифт '{choice}' не поддерживается. Доступные варианты: {string.Join(", ", Choices)}");
        }
    }

    private static IReadOnlyDictionary<string, IFontChoiceStrategy> CreateMap(IEnumerable<IFontChoiceStrategy> src)
    {
        var dict = new Dictionary<string, IFontChoiceStrategy>(StringComparer.OrdinalIgnoreCase);

        var fontChoiceStrategies = src as IFontChoiceStrategy[] ?? src.ToArray();
        foreach (var s in fontChoiceStrategies)
            dict.Add(s.Key, s);
        
        dict.Add("", fontChoiceStrategies.First());

        return dict;
    }

    private static string Normalize(string? value) =>
        string.Concat(value).Trim().ToLowerInvariant();
}
