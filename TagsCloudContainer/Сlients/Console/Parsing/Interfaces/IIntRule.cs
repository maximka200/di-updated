namespace TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

public interface IIntRule
{
    string Label { get; }
    int Validate(int value);
}
