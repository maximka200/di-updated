using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients;

public class ClientEqualsStrategy : IArgStrategy
{
    private static readonly IReadOnlyDictionary<string, Action<string, ParseContext>> Map =
        new Dictionary<string, Action<string, ParseContext>>(StringComparer.OrdinalIgnoreCase)
        {
            [ClientSelectionParser.ClientFlag] = static (value, ctx) => ctx.SetClientKey(value)
        };

    public ArgStep Handle(string[] args, int index, ParseContext ctx)
    {
        var token = args[index];

        try
        {
            var parts = token.Split('=', 2);
            var value = parts[1];
            Map[parts[0]](value, ctx);
            return ArgStep.Consumed(1);
        }
        catch (IndexOutOfRangeException)
        {
            return ArgStep.Unhandled;
        }
        catch (KeyNotFoundException)
        {
            return ArgStep.Unhandled;
        }
    }
}
