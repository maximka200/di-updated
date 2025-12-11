using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface ITagCloudGenerator
{
    void Generate(TagCloudGenerationRequest request);
}