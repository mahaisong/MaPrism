// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.PrismServiceProviderFactory
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using Microsoft.Extensions.DependencyInjection;
using System;

#nullable enable
namespace Prism.Ioc
{
  public class PrismServiceProviderFactory : IServiceProviderFactory<IContainerExtension>
  {
    private Action<IContainerExtension> _registerTypes { get; }

    private Lazy<IContainerExtension> _currentContainer { get; }

    public PrismServiceProviderFactory(Action<IContainerExtension> registerTypes)
    {
      this._registerTypes = registerTypes;
      this._currentContainer = new Lazy<IContainerExtension>((Func<IContainerExtension>) (() => ContainerLocator.Current));
    }

    public PrismServiceProviderFactory(IContainerExtension containerExtension)
    {
      this._registerTypes = (Action<IContainerExtension>) (_ => { });
      this._currentContainer = new Lazy<IContainerExtension>((Func<IContainerExtension>) (() => containerExtension));
    }

        /// <summary>
        /// 这里的写法明显是抄的微软的HostBuilder
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
    public IContainerExtension CreateBuilder(IServiceCollection services)
    {
      IContainerExtension container = this._currentContainer.Value;
      container.Populate(services);
      this._registerTypes(container);
      return container;
    }

    public IServiceProvider CreateServiceProvider(IContainerExtension containerExtension)
    {
      return containerExtension.CreateServiceProvider();
    }
  }
}
