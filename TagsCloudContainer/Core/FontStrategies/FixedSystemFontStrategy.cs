using SixLabors.Fonts;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core.FontStrategies;

public abstract class FixedSystemFontStrategy : IFontChoiceStrategy
{
    private readonly Lazy<FontFamily> lazy;

    protected FixedSystemFontStrategy()
        => lazy = new Lazy<FontFamily>(() => Find(SystemFontName), isThreadSafe: true);

    public abstract string Key { get; }
    protected abstract string SystemFontName { get; }

    public FontFamily Resolve() => lazy.Value;

    private static FontFamily Find(string name)
    {
        try
        {
            return SystemFonts.Collection.Families
                .First(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException(
                $"Системный шрифт '{name}' не найден. Поставь его в систему или замени стратегию на доступный шрифт.");
        }
    }
}
