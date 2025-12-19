using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients.ClientParserStrategies;

public sealed class RestArgStrategy : IArgStrategy
{
    public ArgStep Handle(string[] args, int index, ParseContext ctx)
    {
        ctx.AddRest(args[index]);
        return ArgStep.Consumed(1);
    }
}