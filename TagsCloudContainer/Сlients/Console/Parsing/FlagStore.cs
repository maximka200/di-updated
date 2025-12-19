namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class FlagStore
{
    private static readonly IReadOnlyDictionary<string, string> Allowed =
        CliFlags.All.ToDictionary(x => x, x => x, StringComparer.OrdinalIgnoreCase);

    private static readonly IReadOnlyDictionary<bool, Action<string>> RequireKnown =
        new Dictionary<bool, Action<string>>
        {
            [true] = _ => { },
            [false] = k => throw new Exception($"Неизвестный флаг: {k}")
        };

    private static readonly IReadOnlyDictionary<bool, Action<string>> RequireNonEmpty =
        new Dictionary<bool, Action<string>>
        {
            [true] = _ => { },
            [false] = k => throw new Exception($"Пустое значение для {k}")
        };

    public static void Put(IDictionary<string, string?> flags, string key, string? value)
    {
        try { _ = Allowed[key]; }
        catch (KeyNotFoundException) { RequireKnown[false](key); }

        var trimmed = string.Concat(value).Trim();
        RequireNonEmpty[!string.IsNullOrWhiteSpace(trimmed)](key);

        try { flags.Add(key, trimmed); }
        catch (ArgumentException) { throw new Exception($"Флаг указан дважды: {key}"); }
    }
}