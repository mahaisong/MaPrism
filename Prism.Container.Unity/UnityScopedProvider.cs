// Decompiled with JetBrains decompiler
// Type: Prism.Container.Unity.UnityScopedProvider
// Assembly: Prism.Container.Unity, Version=9.0.114.15915, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: E01EFCD6-DCE7-4021-B1F2-2FA2B4C29C0F
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.unity\9.0.114\lib\net8.0\Prism.Container.Unity.dll

using Prism.Ioc;
using Prism.Ioc.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity;
using Unity.Resolution;

#nullable enable
namespace Prism.Container.Unity
{
  internal class UnityScopedProvider : IScopedProvider, IContainerProvider, IDisposable
  {
    private readonly List<IScopedProvider> _children = new List<IScopedProvider>();

    public UnityScopedProvider(IUnityContainer container)
    {
      this.Container = container;
      container.Resolve<ContainerProviderLocator>().Current = (IContainerProvider) this;
    }

    public IUnityContainer? Container { get; private set; }

    public bool IsAttached { get; set; }

    public IScopedProvider CurrentScope => (IScopedProvider) this;

    public IScopedProvider CreateChildScope()
    {
      UnityScopedProvider childScope = this.Container != null ? new UnityScopedProvider(this.Container.CreateChildContainer()) : throw new InvalidOperationException("This container scope has been disposed.");
      this._children.Add((IScopedProvider) childScope);
      return (IScopedProvider) childScope;
    }

    public IScopedProvider CreateScope() => (IScopedProvider) this;

    public void Dispose()
    {
      foreach (IDisposable child in this._children)
        child.Dispose();
      this._children.Clear();
      this.Container?.Dispose();
      this.Container = (IUnityContainer) null;
    }

    public object Resolve(Type type) => this.Resolve(type, Array.Empty<(Type, object)>());

    public object Resolve(Type type, string name)
    {
      return this.Resolve(type, name, Array.Empty<(Type, object)>());
    }

    public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
    {
      try
      {
        if (this.Container == null)
          throw new InvalidOperationException("This container scope has been disposed.");
        DependencyOverride[] array = ((IEnumerable<(Type, object)>) parameters).Select<(Type, object), DependencyOverride>((Func<(Type, object), DependencyOverride>) (p => new DependencyOverride(p.Item1, p.Item2))).ToArray<DependencyOverride>();
        return this.Container.Resolve(type, (ResolverOverride[]) array);
      }
      catch (Exception ex)
      {
        throw new ContainerResolutionException(type, ex, (IContainerProvider) this);
      }
    }

    public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
    {
      try
      {
        if (this.Container == null)
          throw new InvalidOperationException("This container scope has been disposed.");
        if (!this.Container.IsRegistered(type, name))
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
          interpolatedStringHandler.AppendLiteral("No registered type ");
          interpolatedStringHandler.AppendFormatted(type.Name);
          interpolatedStringHandler.AppendLiteral(" with the key ");
          interpolatedStringHandler.AppendFormatted(name);
          interpolatedStringHandler.AppendLiteral(".");
          throw new KeyNotFoundException(interpolatedStringHandler.ToStringAndClear());
        }
        DependencyOverride[] array = ((IEnumerable<(Type, object)>) parameters).Select<(Type, object), DependencyOverride>((Func<(Type, object), DependencyOverride>) (p => new DependencyOverride(p.Item1, p.Item2))).ToArray<DependencyOverride>();
        return this.Container.Resolve(type, name, (ResolverOverride[]) array);
      }
      catch (Exception ex)
      {
        throw new ContainerResolutionException(type, name, ex, (IContainerProvider) this);
      }
    }
  }
}
