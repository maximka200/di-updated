using System.Text;
using TagsCloudContainer.Сlients;
using TagsCloudContainer.Сlients.Console;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer;

internal static class Program
{
    private static readonly IClientStrategy[] Strategies = [
        new ConsoleClientStrategy(),
    ];

    private static int Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var selector = new ClientStrategySelector(
            Strategies, new ClientSelectionParser());

        return selector.Run(args);
    }
}