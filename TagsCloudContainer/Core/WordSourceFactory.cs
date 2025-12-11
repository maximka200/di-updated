using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class WordsSourceFactory : IWordsSourceFactory
{
    public IWordsSource Create(SourceSettings settings)
    {
        if (settings.Format.Equals("txt", StringComparison.InvariantCultureIgnoreCase))
            return new TxtWordsSource(settings.Path);

        throw new NotSupportedException(
            $"Формат источника '{settings.Format}' не поддерживается");
    }
}