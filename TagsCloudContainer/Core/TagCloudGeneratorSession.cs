using Autofac;
using TagsCloudContainer.Core.Interfaces;

namespace TagsCloudContainer.Core;

public sealed class TagCloudGeneratorSession : IDisposable
{
    private readonly ILifetimeScope scope;

    internal TagCloudGeneratorSession(ILifetimeScope scope)
    {
        this.scope = scope;
    }

    public void Dispose() => scope.Dispose();
}