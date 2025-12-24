using Autofac;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Сlients;

public class ScopedGenerator(ILifetimeScope scope) : ITagCloudGenerator, IDisposable
{
    private readonly ITagCloudGenerator inner = scope.Resolve<ITagCloudGenerator>();

    public void Generate(Core.Domains.TagCloudGenerationRequest request) => inner.Generate(request);

    public void Dispose() => scope.Dispose();
}