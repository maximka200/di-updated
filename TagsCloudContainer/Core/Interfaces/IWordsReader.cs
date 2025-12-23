using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsReader
{
    IEnumerable<string> Read(TagCloudGenerationRequest request);
}