 
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using System;

#nullable enable
namespace Prism.Container.DryIoc
{
  internal sealed class DryIocServiceScope : IServiceScope, IDisposable
  {
    private readonly IResolverContext _resolverContext;

    public IServiceProvider ServiceProvider => (IServiceProvider) this._resolverContext;

    public DryIocServiceScope(IResolverContext resolverContext)
    {
      this._resolverContext = resolverContext;
    }

    public void Dispose() => ((IDisposable) this._resolverContext).Dispose();
  }
}
