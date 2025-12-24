using System.Text;
using Autofac;
using TagsCloudContainer.Сlients;

namespace TagsCloudContainer;

public static class Program
{
    public static int Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var container = ContainerBootstrapper.Build();

        var selector = container.Resolve<ClientStrategySelector>();
        return selector.Run(args);
    }
}