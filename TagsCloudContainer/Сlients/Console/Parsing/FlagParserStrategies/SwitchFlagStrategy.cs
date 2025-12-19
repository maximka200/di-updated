using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing.FlagParserStrategies;

public class SwitchFlagStrategy : IArgConsoleStrategy
{
    private static readonly IReadOnlyDictionary<string, Action<IDictionary<string, string?>>> map =
        new Dictionary<string, Action<IDictionary<string, string?>>>(StringComparer.OrdinalIgnoreCase)
        {
            [CliFlags.Desc] = static flags =>
                FlagStore.Put(flags, CliFlags.Desc, "true")
        };

    public ArgStep Handle(string[] args, int index, IDictionary<string, string?> flags)
    {
        var token = args[index];

        try
        {
            map[token](flags);
            return ArgStep.Consumed(1);
        }
        catch (KeyNotFoundException)
        {
            return ArgStep.Unhandled;
        }
    }
}