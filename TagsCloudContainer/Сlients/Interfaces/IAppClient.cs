namespace TagsCloudContainer.Сlients.Interfaces;

public interface IClientStrategy
{
    string Key { get; }    
    int Run(string[] args);
}