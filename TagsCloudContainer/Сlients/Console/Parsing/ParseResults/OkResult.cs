namespace TagsCloudContainer.Сlients.Console.Parsing.ParseResults;

public class OkResult(ConsoleOptions? value) : ParseResult
{
    public override bool Apply(out ConsoleOptions? options, out string error)
    {
        options = value;
        error = string.Empty;
        return true;
    }
}