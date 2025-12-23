using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class WordsReader(IEnumerable<IWordsSource> sources) : IWordsReader
{
    public IEnumerable<string> Read(TagCloudGenerationRequest request)
    {
        var source = WordsSourceFactory.Create(request.SourceSettings, sources.ToArray());
        return source.GetWords(request.SourceSettings.Path);
    }
}