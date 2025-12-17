using Autofac;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core;

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

        var stopWordsPath = Path.Combine(AppContext.BaseDirectory, "stop-words.txt");
        builder.RegisterModule(new TagCloudBuilder(new Point(500, 500), stopWordsPath));

        builder.RegisterType<ConsoleClient>()
            .AsSelf()
            .SingleInstance();

        return builder.Build();
    }
}