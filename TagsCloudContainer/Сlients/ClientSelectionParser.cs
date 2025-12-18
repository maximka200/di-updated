using TagsCloudContainer.Сlients.Exceptions;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients;

public sealed class ClientSelectionParser : IClientSelectionParser
{
    private const string ClientFlag = "--client";

    public ClientSelection Parse(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        string? clientKey = null;
        var rest = new List<string>(args.Length);

        for (var i = 0; i < args.Length; i++)
        {
            var a = args[i];
            
            if (a.StartsWith(ClientFlag + "=", StringComparison.OrdinalIgnoreCase))
            {
                EnsureNotSet(clientKey);

                clientKey = a[(ClientFlag.Length + 1)..].Trim();
                EnsureValid(clientKey);

                continue;
            }
            
            if (a.Equals(ClientFlag, StringComparison.OrdinalIgnoreCase))
            {
                EnsureNotSet(clientKey);

                if (i + 1 >= args.Length)
                    throw new CommandLineException($"Ожидалось значение после {ClientFlag}");

                clientKey = args[++i].Trim();
                EnsureValid(clientKey);

                continue;
            }
            
            rest.Add(a);
        }

        if (string.IsNullOrWhiteSpace(clientKey))
            throw new CommandLineException($"Не задан клиент. Используй {ClientFlag} <id> или {ClientFlag}=<id>.");

        return new ClientSelection(clientKey, rest.ToArray());
    }

    private static void EnsureNotSet(string? current)
    {
        if (!string.IsNullOrWhiteSpace(current))
            throw new CommandLineException("Флаг --client указан дважды.");
    }

    private static void EnsureValid(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new CommandLineException("Пустое значение у --client.");
    }
}