using System.Drawing;
using Autofac;
using TagCloud;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                return 1;
            }

            var inputPath = args[0];
            var outputPath = args.Length > 1 ? args[1] : "cloud.png";

            var width = 800;
            var height = 600;

            switch (args.Length)
            {
                case > 2 when !int.TryParse(args[2], out width):
                    Console.WriteLine("Некорректная ширина изображения");
                    return 1;
                case > 3 when !int.TryParse(args[3], out height):
                    Console.WriteLine("Некорректная высота изображения");
                    return 1;
                default:
                    try
                    {
                        var container = BuildContainer();
                        using var scope = container.BeginLifetimeScope();

                        var generator = scope.Resolve<ITagCloudGenerator>();

                        var request = new TagCloudGenerationRequest
                        {
                            SourceSettings = new SourceSettings(inputPath, "txt"),
                            
                            LayoutSettings = new LayoutSettings
                            {
                                ImageSize = new Size(width, height),
                                MinFontSize = 10,
                                MaxFontSize = 60
                            },

                            OutputPath = outputPath
                        };

                        generator.Generate(request);

                        Console.WriteLine($"Облако тегов успешно сохранено в файл: {outputPath}");
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Во время генерации облака произошла ошибка:");
                        Console.WriteLine(ex);
                        return 1;
                    }
            }
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<WordsSourceFactory>()
                .As<IWordsSourceFactory>()
                .SingleInstance();
            
            builder.RegisterType<FileStopWordsProvider>()
                .As<IStopWordsProvider>()
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

            builder.RegisterType<TagSizeCalculator>()
                .As<ITagSizeCalculator>()
                .SingleInstance();
                
            builder.Register(_ =>
                    new CircularCloudLayouterWrapper(new Point(500, 500))) 
                .As<ICircularCloudLayouterWrapper>()
                .SingleInstance();

            builder.RegisterType<CloudPositionedTags>()
                .As<ICloudPositionedTags>()
                .WithParameter("minFontSize", 12f)
                .WithParameter("maxFontSize", 64f)
                .SingleInstance();
            
            builder.RegisterType<TagCloudGenerator>()
                .As<ITagCloudGenerator>()
                .SingleInstance();

            return builder.Build();
        }
    }
}
