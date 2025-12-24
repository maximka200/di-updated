using TagsCloudContainer.Сlients.ClientParserStrategies;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using TagsCloudContainer.Сlients.Domains;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients;

public class ClientSelectionParser : IClientSelectionParser
{
    public const string ClientFlag = "--client";

    private static readonly IArgStrategy[] Strategies =
    [
        new ClientEqualsStrategy(),
        new ClientNextTokenStrategy(),
        new RestArgStrategy()
    ];

    public ClientSelection Parse(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var ctx = new ParseContext(args.Length);

        var i = 0;
        while (i < args.Length)
        {
            var step = Strategies.Aggregate(
                ArgStep.Unhandled,
                (acc, s) => acc.OrElse(() => s.Handle(args, i, ctx)));

            i = step.NextIndex(i);
        }

        return ctx.Build();
    }
}
