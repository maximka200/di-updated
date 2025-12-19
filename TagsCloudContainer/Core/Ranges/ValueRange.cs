namespace TagsCloudContainer.Core.Ranges;

public sealed class ValueRange(int min, int max) : MaybeRange
{
    public override (int Min, int Max) GetOrYieldBreak() => (min, max);
}