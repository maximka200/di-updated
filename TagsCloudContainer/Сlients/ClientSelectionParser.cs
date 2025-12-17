using TagsCloudContainer.Сlients.Exceptions;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients;

public sealed class ClientSelectionParser : IClientSelectionParser
{
    private const string ClientOpt = "--client";

    public ClientSelection Parse(string[] args)
    {
        string? clientKey = null;
        var rest = new List<string>();

        for (var i = 0; i < args.Length; i++)
        {
            var a = args[i];
            
            if (a.StartsWith("--", StringComparison.Ordinal))
            {
                if (a.StartsWith(ClientOpt + "=", StringComparison.OrdinalIgnoreCase))
                {
                    EnsureNotSet(clientKey);
                    clientKey = a[(ClientOpt.Length + 1)..].Trim();
                    EnsureValue(clientKey);
                    continue;
                }

                if (a.Equals(ClientOpt, StringComparison.OrdinalIgnoreCase))
                {
                    EnsureNotSet(clientKey);
                    if (i + 1 >= args.Length)
                        throw new CommandLineException("Ожидалось значение после --client.");

                    clientKey = args[++i].Trim();
                    EnsureValue(clientKey);
                    continue;
                }

                throw new CommandLineException($"Неизвестный флаг: {a}");
            }

            rest.Add(a);
        }

        if (clientKey is null)
            throw new CommandLineException("Не указан клиент. Используй: --client <id> или --client=<id>.");

        return new ClientSelection(clientKey, rest.ToArray());
    }

    private static void EnsureNotSet(string? current)
    {
        if (current is not null)
            throw new CommandLineException("Ключ --client указан больше одного раза.");
    }

    private static void EnsureValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new CommandLineException("Значение --client не может быть пустым.");
    }
}