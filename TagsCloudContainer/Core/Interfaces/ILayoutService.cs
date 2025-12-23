using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface ILayoutService
{
    IReadOnlyCollection<PositionedTag> Layout(TagCloudGenerationRequest request, IReadOnlyCollection<Tag> tags);
}