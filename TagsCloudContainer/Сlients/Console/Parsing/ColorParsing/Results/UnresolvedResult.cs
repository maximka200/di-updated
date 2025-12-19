using SixLabors.ImageSharp;

namespace TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Results;

public class UnresolvedResult : ColorResult
{
    public override ColorResult OrElse(Func<ColorResult> next) => next();
    public override Color Unwrap(string key, string raw) =>
        throw new Exception($"Некорректный {key}: {raw}. Пример: {key} #ffffff");
}