using SixLabors.ImageSharp;

namespace TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Results;

public class ResolvedResult(Color color) : ColorResult
{
    public override ColorResult OrElse(Func<ColorResult> next) => this;
    public override Color Unwrap(string key, string raw) => color;
}