namespace TagsCloudContainer.Сlients.Console.Parsing.ParseResults;

public abstract class ParseResult
{
    public static ParseResult Ok(ConsoleOptions? options) => new OkResult(options);
    public static ParseResult Fail(string error) => new FailResult(error);

    public abstract bool Apply(out ConsoleOptions? options, out string error);
}