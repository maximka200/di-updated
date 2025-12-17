namespace TagsCloudContainer;

public interface IClientStrategy
{
    string Key { get; }    
    int Run(string[] args);
}