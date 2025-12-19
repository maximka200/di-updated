namespace TagsCloudContainer.Сlients.Domains;

public class ParseContext(int capacity)
{
    private ClientKeyState state = ClientKeyState.Unset;
    private readonly List<string> rest = new(capacity);

    public void AddRest(string arg) => rest.Add(arg);

    public void SetClientKey(string raw)
    {
        var key = ClientKey.Parse(raw);
        state = state.Set(key);
    }

    public ClientSelection Build() => new(state.GetOrThrow(), rest.ToArray());
}

