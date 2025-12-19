using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using TagsCloudContainer.Сlients.Console.Parsing.PreCheckResults;

namespace TagsCloudContainer.Сlients.Console.Parsing;

public class EmptyArgsIsHelpPreCheck : IPreCheck
{
    public PreCheckResult Check(string[] args)
    {
        try
        {
            _ = args[0];
            return PreCheckResult.Continue;
        }
        catch (IndexOutOfRangeException)
        {
            return PreCheckResult.Stop("help");
        }
    }
}