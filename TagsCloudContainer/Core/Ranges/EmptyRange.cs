namespace TagsCloudContainer.Core.Ranges;

public sealed class EmptyRange : MaybeRange
{
    public override (int Min, int Max) GetOrYieldBreak() => throw new Exception("Yield break");
}