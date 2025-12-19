namespace TagsCloudContainer.Core.Ranges;

public abstract class MaybeRange
{
    public static readonly MaybeRange Empty = new EmptyRange();
    public static MaybeRange Range(int min, int max) => new ValueRange(min, max);

    public abstract (int Min, int Max) GetOrYieldBreak();
}