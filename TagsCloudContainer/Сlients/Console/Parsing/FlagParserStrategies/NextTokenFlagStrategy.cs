using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing.FlagParserStrategies;

public class NextTokenFlagStrategy : IArgConsoleStrategy
{
    public ArgStep Handle(string[] args, int index, IDictionary<string, string?> flags)
    {
        var key = args[index];

        try
        {
            var value = args[index + 1];
            Ensure.NextTokenIsValue(value, key);

            FlagStore.Put(flags, key, value);
            return ArgStep.Consumed(2);
        }
        catch (IndexOutOfRangeException)
        {
            throw new Exception($"Ожидалось значение после {key}");
        }
    }
}