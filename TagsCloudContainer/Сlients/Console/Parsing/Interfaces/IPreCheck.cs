using TagsCloudContainer.Сlients.Console.Parsing.PreCheckResults;

namespace TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

internal interface IPreCheck
{
    PreCheckResult Check(string[] args);
}