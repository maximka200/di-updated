using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing.FlagParserStrategies;

public class EqualsFlagStrategy : IArgConsoleStrategy
{
    public ArgStep Handle(string[] args, int index, IDictionary<string, string?> flags)
    {
        var token = args[index];

        try
        {
            var parts = token.Split('=', 2, StringSplitOptions.None);
            var key = parts[0];
            var value = parts[1];

            FlagStore.Put(flags, key, value);
            return ArgStep.Consumed(1);
        }
        catch (IndexOutOfRangeException)
        {
            return ArgStep.Unhandled;
        }
    }
}