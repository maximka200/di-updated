namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsPreprocessor
{
    IEnumerable<string> Process(IEnumerable<string> words);
}