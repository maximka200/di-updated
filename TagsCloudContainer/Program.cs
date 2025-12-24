using System.Text;
using Autofac;
using TagsCloudContainer.Сlients;
using TagsCloudContainer.Сlients.Console;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer;

public static class Program
{
    private static int Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var builder = new ContainerBuilder();
        
        builder.RegisterType<AutofacTagCloudGeneratorFactory>()
            .As<ITagCloudGeneratorFactory>()
            .InstancePerDependency();
        
        builder.RegisterType<ClientSelectionParser>()
            .As<IClientSelectionParser>()
            .SingleInstance();
        
        builder.RegisterType<ClientStrategySelector>()
            .AsSelf()
            .SingleInstance();
        
        builder.RegisterType<ConsoleClient>()
            .As<IClient>()
            .InstancePerDependency();
        
        builder.RegisterType<ConsoleClientStrategy>()
            .As<IClientStrategy>()
            .SingleInstance();
        
        using var container = builder.Build();

        var selector = container.Resolve<ClientStrategySelector>();
        return selector.Run(args);
    }
}