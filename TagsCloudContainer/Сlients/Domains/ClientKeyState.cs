using TagsCloudContainer.Сlients.Exceptions;

namespace TagsCloudContainer.Сlients.Domains;

public abstract class ClientKeyState
{
    public static readonly ClientKeyState Unset = new UnsetState();

    public abstract ClientKeyState Set(string key);
    public abstract string GetOrThrow();

    private sealed class UnsetState : ClientKeyState
    {
        public override ClientKeyState Set(string key) => new SetState(key);

        public override string GetOrThrow() =>
            throw new CommandLineException($"Не задан клиент. Используй {ClientSelectionParser.ClientFlag} <id> или {ClientSelectionParser.ClientFlag}=<id>.");
    }

    private sealed class SetState(string key) : ClientKeyState
    {
        public override ClientKeyState Set(string k) =>
            throw new CommandLineException("Флаг указан дважды");

        public override string GetOrThrow() => key;
    }
}