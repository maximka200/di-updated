namespace TagsCloudContainer.Сlients.Console.Parsing.ParseResults;

public class FailResult(string message) : ParseResult
{
    public override bool Apply(out ConsoleOptions? options, out string error)
    {
        options = default;
        error = message;
        return false;
    }
}