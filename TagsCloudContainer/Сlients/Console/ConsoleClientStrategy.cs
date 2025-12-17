using Autofac;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients.Console;

public sealed class ConsoleClientStrategy : IClientStrategy
{
    public string Key => "console";

    public int Run(string[] args)
    {
        try
        {
            using var container = BuildContainer();
            using var scope = container.BeginLifetimeScope();

            var app = scope.Resolve<ConsoleClient>();
            return app.Run(args);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Ошибка при запуске консольного клиента:");
            System.Console.WriteLine(ex);
            return 1;
        }
    }

    private static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<TagCloudGeneratorFactory>()
            .As<ITagCloudGeneratorFactory>()
            .SingleInstance();

        builder.RegisterType<ConsoleClient>()
            .AsSelf()
            .SingleInstance();

        return builder.Build();
    }
}