using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class TagsBuilder(IWordFrequencyAnalyzer analyzer) : ITagsBuilder
{
    private readonly IWordFrequencyAnalyzer analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));

    public IReadOnlyCollection<Tag> Build(IEnumerable<string> words)
    {
        var freq = analyzer.GetFrequencies(words);
        return freq.Select(x => new Tag(x.Key, x.Value)).ToList();
    }
}
