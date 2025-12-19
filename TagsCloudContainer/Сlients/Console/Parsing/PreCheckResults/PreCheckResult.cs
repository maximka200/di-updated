using TagsCloudContainer.Сlients.Console.Parsing.ParseResults;

namespace TagsCloudContainer.Сlients.Console.Parsing.PreCheckResults;

public abstract class PreCheckResult
{
    public static readonly PreCheckResult Continue = new ContinueResult();
    public static PreCheckResult Stop(string error) => new StopResult(error);

    public abstract PreCheckResult OrElse(Func<PreCheckResult> next);
    public abstract ParseResult Then(Func<ParseResult> next);
}