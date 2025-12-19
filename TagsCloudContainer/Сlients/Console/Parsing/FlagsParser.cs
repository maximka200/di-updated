using TagsCloudContainer.Сlients.Console.Parsing.FlagParserStrategies;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing;

public static class FlagsParser
{
    private static readonly IArgConsoleStrategy[] Strategies =
    [
        new SwitchFlagStrategy(),
        new EqualsFlagStrategy(),
        new NextTokenFlagStrategy()
    ];

    public static IReadOnlyDictionary<string, string?> Parse(string[] args)
    {
        var flags = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        var i = 0;

        while (i < args.Length)
        {
            var a = args[i];
            Ensure.StartsWithFlagPrefix(a);

            var step = Strategies.Aggregate(
                ArgStep.Unhandled,
                (acc, s) => acc.OrElse(() => s.Handle(args, i, flags)));

            i = step.NextIndex(i);
        }

        return flags;
    }
}
