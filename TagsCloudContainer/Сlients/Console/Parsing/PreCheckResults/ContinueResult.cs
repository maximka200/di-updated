using TagsCloudContainer.Сlients.Console.Parsing.ParseResults;

namespace TagsCloudContainer.Сlients.Console.Parsing.PreCheckResults;

public class ContinueResult : PreCheckResult
{
    public override PreCheckResult OrElse(Func<PreCheckResult> next) => next();
    public override ParseResult Then(Func<ParseResult> next) => next();
}