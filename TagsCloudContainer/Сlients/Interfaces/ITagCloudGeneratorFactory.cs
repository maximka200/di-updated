using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients.Interfaces;

public interface ITagCloudGeneratorFactory
{
    ITagCloudGenerator Create(TagCloudRuntimeSettings settings);
}