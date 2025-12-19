using TagsCloudContainer.Сlients.Domains;
using TagsCloudContainer.Сlients.Exceptions;

namespace TagsCloudContainer.Сlients.ClientParserStrategies;

public sealed class ClientNextTokenStrategy : IArgStrategy
{
    private static readonly IReadOnlyDictionary<string, Func<string[], int, ParseContext, ArgStep>> Map =
        new Dictionary<string, Func<string[], int, ParseContext, ArgStep>>(StringComparer.OrdinalIgnoreCase)
        {
            [ClientSelectionParser.ClientFlag] = static (args, index, ctx) =>
            {
                var value = args[index + 1];
                ctx.SetClientKey(value);
                return ArgStep.Consumed(2);
            }
        };

    public ArgStep Handle(string[] args, int index, ParseContext ctx)
    {
        var token = args[index];

        try
        {
            return Map[token](args, index, ctx);
        }
        catch (IndexOutOfRangeException)
        {
            throw new CommandLineException($"Ожидалось значение после {ClientSelectionParser.ClientFlag}");
        }
        catch (KeyNotFoundException)
        {
            return ArgStep.Unhandled;
        }
    }
}
