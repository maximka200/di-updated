using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients;

public interface IArgStrategy
{
    ArgStep Handle(string[] args, int index, ParseContext ctx);
}