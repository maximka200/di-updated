using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using TagsCloudContainer.Сlients.Console.Parsing.PreCheckResults;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal class HelpFlagPreCheck(string token) : IPreCheck
{
    public PreCheckResult Check(string[] args)
    {
        try
        {
            _ = args.First(a => a.Equals(token, StringComparison.OrdinalIgnoreCase));
            return PreCheckResult.Stop("help");
        }
        catch (InvalidOperationException)
        {
            return PreCheckResult.Continue;
        }
    }
}