using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients.Console;

public class ConsoleClientStrategy(IClient client) : IClientStrategy
{
    public string Key => "console";

    public int Run(string[] args)
    {
        try
        {
            return client.Run(args);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Ошибка при запуске консольного клиента:");
            System.Console.WriteLine(ex);
            return 1;
        }
    }
}