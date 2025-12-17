using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Interfaces;


public interface ITagCloudGeneratorFactory
{
    TagCloudGeneratorSession Create(Point center, string stopWordsPath);
}