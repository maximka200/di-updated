namespace TagsCloudContainer.Сlients.Interfaces;

public interface IClientSelectionParser
{
    ClientSelection Parse(string[] args);
}