using TagsCloudContainer.Сlients.Console.Parsing.ParseResults;

namespace TagsCloudContainer.Сlients.Console.Parsing.PreCheckResults;

public class StopResult(string error) : PreCheckResult
{
    public override PreCheckResult OrElse(Func<PreCheckResult> next) => this;
    public override ParseResult Then(Func<ParseResult> next) => ParseResult.Fail(error);
}