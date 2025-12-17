using TagsCloudContainer.Сlients.Console;
using TagsCloudContainer.Сlients.GUI;

namespace TagsCloudContainer;

internal static class Program
{
    private static int Main(string[] args)
    {
        var selector = new ClientStrategySelector(
        [
            new ConsoleClientStrategy(),
            new GuiClientStrategy()
        ]);

        return selector.Run(args);
    }
}