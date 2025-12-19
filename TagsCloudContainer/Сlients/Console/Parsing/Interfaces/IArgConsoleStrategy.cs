namespace TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

public interface IArgConsoleStrategy
{
    ArgStep Handle(string[] args, int index, IDictionary<string, string?> flags);
}