using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing;

public static class FlagsParser
{
    private static readonly IArgConsoleStrategy[] Strategies =
    [
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

    private class EqualsFlagStrategy : IArgConsoleStrategy
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

    private class NextTokenFlagStrategy : IArgConsoleStrategy
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
}
