namespace TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

public interface IFloatRule
{
    string Label { get; }
    float Validate(float value);
}