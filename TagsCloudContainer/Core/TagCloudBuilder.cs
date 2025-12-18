using Autofac;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public class TagCloudBuilder(Point center, string? stopWordsPath)
    : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WordsSourceFactory>()
            .SingleInstance();
        
        builder.RegisterType<TagSizeCalculator>()
            .As<ITagSizeCalculator>()
            .SingleInstance();

        builder.RegisterType<FileStopWordsProvider>()
            .As<IStopWordsProvider>()
            .WithParameter("stopWordsPath", stopWordsPath ?? string.Empty)
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

        builder.RegisterType<WordFrequencyAnalyzer>()
            .As<IWordFrequencyAnalyzer>()
            .SingleInstance();

        builder.Register(_ => SystemFonts.Collection.Families.First())
            .As<FontFamily>()
            .SingleInstance();
        
        builder.Register(_ => new CircularCloudLayouterWrapper(center))
            .As<ICircularCloudLayouterWrapper>()
            .SingleInstance();

        builder.RegisterType<CloudPositionedTags>()
            .As<ICloudPositionedTags>()
            .SingleInstance();

        builder.RegisterType<TagCloudGenerator>()
            .As<ITagCloudGenerator>()
            .SingleInstance();
    }
}
