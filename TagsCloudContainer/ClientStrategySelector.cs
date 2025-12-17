namespace TagsCloudContainer;

public sealed class ClientStrategySelector
{
    private readonly IReadOnlyDictionary<string, IClientStrategy> strategies;

    public ClientStrategySelector(IEnumerable<IClientStrategy> strategies)
    {
        this.strategies = strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
    }

    public int Run(string[] args)
    {
        var (key, rest) = ParseClient(args);

        if (strategies.TryGetValue(key, out var strategy)) return strategy.Run(rest);
        Console.WriteLine($"Неизвестный клиент: {key}. Доступно: {string.Join(", ", strategies.Keys)}");
        return 1;

    }

    private static (string key, string[] rest) ParseClient(string[] args)
    {
        var key = "console";
        var rest = new List<string>();

        for (var i = 0; i < args.Length; i++)
        {
            var a = args[i];

            if (a.Equals("--gui", StringComparison.OrdinalIgnoreCase)) { key = "gui"; continue; }
            if (a.Equals("--console", StringComparison.OrdinalIgnoreCase)) { key = "console"; continue; }

            if (a.StartsWith("--client=", StringComparison.OrdinalIgnoreCase))
            {
                key = a.Substring("--client=".Length).Trim();
                continue;
            }

            if (a.Equals("--client", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                key = args[++i].Trim();
                continue;
            }

            rest.Add(a);
        }

        return (key.ToLowerInvariant(), rest.ToArray());
    }
}