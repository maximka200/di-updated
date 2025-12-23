using Autofac;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class TagCloudBuilder(Point center, string? stopWordsPath, 
    IOutputFormat[] outputFormats, IWordsSource[] wordsSources) : Module
{
    private const string StopWordsParamName = "stopWordsPath";
    
    protected override void Load(ContainerBuilder builder)
    {
        foreach (var fmt in outputFormats)
        {
            builder.RegisterInstance(fmt)
                .As<IOutputFormat>()
                .SingleInstance();
        }
        
        foreach (var src in wordsSources)
        {
            builder.RegisterInstance(src)
                .As<IWordsSource>()
                .SingleInstance();
        }
        
        builder.RegisterType<TagSizeCalculator>()
            .As<ITagSizeCalculator>()
            .SingleInstance();

        builder.RegisterType<FileStopWordsProvider>()
            .As<IStopWordsProvider>()
            .WithParameter(StopWordsParamName, stopWordsPath ?? string.Empty)
            .SingleInstance();

        builder.RegisterType<LowerCaseNormalizer>()
            .As<IWordNormalizer>()
            .SingleInstance();

        builder.RegisterType<StopWordsFilter>()
            .As<IWordsFilter>()
            .SingleInstance();

        builder.RegisterType<CompositeWordsPreprocessor>()
            .As<IWordsPreprocessor>()
            .SingleInstance();
        
        builder.RegisterType<WordsReader>()
            .As<IWordsReader>()
            .SingleInstance();

        builder.RegisterType<WordFrequencyAnalyzer>()
            .As<IWordFrequencyAnalyzer>()
            .SingleInstance();

        builder.RegisterType<TagsBuilder>()
            .As<ITagsBuilder>()
            .SingleInstance();
        
        builder.Register(_ => new CircularCloudLayouterWrapper(center))
            .As<ICircularCloudLayouterWrapper>()
            .SingleInstance();

        builder.RegisterType<CloudPositionedTags>()
            .As<ICloudPositionedTags>()
            .SingleInstance();

        builder.RegisterType<LayoutService>()
            .As<ILayoutService>()
            .SingleInstance();
        
        builder.RegisterType<CloudRenderer>()
            .As<ICloudRenderer>()
            .SingleInstance();
        
        builder.RegisterType<ImageSaver>()
            .As<IImageSaver>()
            .SingleInstance();
        
        builder.RegisterType<TagCloudGenerator>()
            .As<ITagCloudGenerator>()
            .SingleInstance();
    }
}
