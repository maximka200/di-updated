using TagsCloudContainer.Core.Domains;

namespace TagsCloudContainer.Core.Interfaces;

public interface ITagsBuilder
{
    IReadOnlyCollection<Tag> Build(IEnumerable<string> words);
}