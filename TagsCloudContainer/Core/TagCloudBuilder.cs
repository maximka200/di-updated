using Autofac;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public sealed class TagCloudBuilder(Point center, string? stopWordsPath, float minFontSize = 12f,
    float maxFontSize = 64f)
    : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WordsSourceFactory>()
            .As<IWordsSourceFactory>()
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
            .WithParameter("minFontSize", minFontSize)
            .WithParameter("maxFontSize", maxFontSize)
            .SingleInstance();

        builder.RegisterType<TagCloudGenerator>()
            .As<ITagCloudGenerator>()
            .SingleInstance();
    }
}
