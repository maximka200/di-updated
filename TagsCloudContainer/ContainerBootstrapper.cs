using Autofac;
using TagsCloudContainer.Сlients;
using TagsCloudContainer.Сlients.Console;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer;

public static class ContainerBootstrapper
{
    public static IContainer Build()
    {
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

        return builder.Build();
    }
}