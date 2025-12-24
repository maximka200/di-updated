using Autofac;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Сlients.Console.Parsing;
using TagsCloudContainer.Сlients.Domains;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients;

public class AutofacTagCloudGeneratorFactory(ILifetimeScope root) : ITagCloudGeneratorFactory
{
    public ITagCloudGenerator Create(TagCloudRuntimeSettings settings)
    {
        var scope = root.BeginLifetimeScope(b =>
            b.RegisterModule(new TagCloudBuilder(
                new Point(settings.CenterX, settings.CenterY),
                settings.StopWordsPath,
                OutputFormatSupport.Sources.ToArray(),
                SourceFormatSupport.Sources.ToArray()
            ))
        );
        
        return new ScopedGenerator(scope);
    }
}