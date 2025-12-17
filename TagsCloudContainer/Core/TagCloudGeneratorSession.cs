using Autofac;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public sealed class TagCloudGeneratorSession : IDisposable
{
    private readonly ILifetimeScope scope;
    public ITagCloudGenerator Generator { get; }

    internal TagCloudGeneratorSession(ILifetimeScope scope, ITagCloudGenerator generator)
    {
        this.scope = scope;
        Generator = generator;
    }

    public void Dispose() => scope.Dispose();
}