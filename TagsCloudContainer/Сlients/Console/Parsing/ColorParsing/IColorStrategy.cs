using TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Results;

namespace TagsCloudContainer.Сlients.Console.Parsing.ColorParsing;

public interface IColorStrategy
{
    ColorResult TryParse(string raw);
}