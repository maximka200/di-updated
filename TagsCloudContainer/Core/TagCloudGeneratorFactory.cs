using Autofac;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;


public sealed class TagCloudGeneratorFactory(ILifetimeScope root) : ITagCloudGeneratorFactory
{
    public TagCloudGeneratorSession Create(Point center, string stopWordsPath)
    {
        var scope = root.BeginLifetimeScope(b =>
            b.RegisterModule(new TagCloudBuilder(center, stopWordsPath))
        );

        var generator = scope.Resolve<ITagCloudGenerator>();
        return new TagCloudGeneratorSession(scope, generator);
    }
}