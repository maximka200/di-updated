using TagsCloudContainer.Сlients.Exceptions;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients;

public sealed class ClientStrategySelector
{
    private readonly IReadOnlyDictionary<string, IClientStrategy> strategies;
    private readonly IClientSelectionParser parser;

    public ClientStrategySelector(IEnumerable<IClientStrategy> strategies, IClientSelectionParser parser)
    {
        this.strategies = strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
        this.parser = parser;
    }

    public int Run(string[] args)
    {
        try
        {
            var selection = parser.Parse(args);

            if (strategies.TryGetValue(selection.ClientKey, out var strategy)) return strategy.Run(selection.RestArgs);
            System.Console.Error.WriteLine(
                $"Неизвестный клиент: {selection.ClientKey}. Доступно: {string.Join(", ", strategies.Keys)}");
            return 1;

        }
        catch (CommandLineException e)
        {
            System.Console.Error.WriteLine(e.Message);
            return 1;
        }
        catch (UnknownClientException e)
        {
            System.Console.Error.WriteLine(e.Message);
            return 1;
        }
    }
}