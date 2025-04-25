 
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using System;

#nullable enable
namespace Prism.Container.DryIoc
{
  internal sealed class DryIocServiceProviderCapabilities : 
    IServiceProviderIsService,
    ISupportRequiredService
  {
    private readonly IContainer _container;

    public DryIocServiceProviderCapabilities(IContainer container) => this._container = container;

    public bool IsService(Type serviceType)
    {
      if (serviceType.IsGenericTypeDefinition)
        return false;
      return serviceType == typeof (IServiceProviderIsService) || serviceType == typeof (ISupportRequiredService) || serviceType == typeof (IServiceScopeFactory) || Registrator.IsRegistered((IRegistrator) this._container, serviceType, (object) null, (FactoryType) 0, (Func<Factory, bool>) null) || serviceType.IsGenericType && Registrator.IsRegistered((IRegistrator) this._container, serviceType.GetGenericTypeDefinition(), (object) null, (FactoryType) 0, (Func<Factory, bool>) null) || Registrator.IsRegistered((IRegistrator) this._container, serviceType, (object) null, (FactoryType) 2, (Func<Factory, bool>) null);
    }

    public object GetRequiredService(Type serviceType)
    {
      return Resolver.Resolve((IResolver) this._container, serviceType);
    }
  }
}
