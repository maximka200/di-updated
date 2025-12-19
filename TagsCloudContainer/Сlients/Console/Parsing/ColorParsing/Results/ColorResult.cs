using SixLabors.ImageSharp;

namespace TagsCloudContainer.Сlients.Console.Parsing.ColorParsing.Results;

public abstract class ColorResult
{
    public static readonly ColorResult Unresolved = new UnresolvedResult();
    public static ColorResult Resolved(Color c) => new ResolvedResult(c);

    public abstract ColorResult OrElse(Func<ColorResult> next);
    public abstract Color Unwrap(string key, string raw);
}
