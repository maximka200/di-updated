namespace TagsCloudContainer.Core.FontStrategies;

public class ArialFontStrategy : FixedSystemFontStrategy
{
    public override string Key => "arial";
    protected override string SystemFontName => "Arial";
}