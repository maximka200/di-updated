using SixLabors.Fonts;

namespace TagsCloudContainer.Core.Interfaces;

public interface IFontChoiceStrategy
{
    string Key { get; }
    FontFamily Resolve();
}
