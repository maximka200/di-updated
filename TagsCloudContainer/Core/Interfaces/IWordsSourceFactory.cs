namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsSourceFactory
{
    IWordsSource Create(SourceSettings settings);
}
