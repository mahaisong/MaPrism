// Decompiled with JetBrains decompiler
// Type: Prism.Container.Unity.UnityContainerExtension
// Assembly: Prism.Container.Unity, Version=9.0.114.15915, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: E01EFCD6-DCE7-4021-B1F2-2FA2B4C29C0F
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.unity\9.0.114\lib\net8.0\Prism.Container.Unity.dll

using Prism.Ioc;
using Prism.Ioc.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity;
using Unity.Lifetime;
using Unity.Resolution;

#nullable enable
namespace Prism.Container.Unity
{
  public sealed class UnityContainerExtension : 
    IContainerExtension<IUnityContainer>,
    IContainerExtension,
    IContainerProvider,
    IContainerRegistry,
    IContainerInfo
  {
    private UnityScopedProvider? _currentScope;

    public IUnityContainer Instance { get; }

    public UnityContainerExtension()
      : this((IUnityContainer) new UnityContainer())
    {
    }

    public UnityContainerExtension(IUnityContainer container)
    {
      this.Instance = container;
      string currentContainer = "CurrentContainer";
      this.Instance.RegisterInstance<UnityContainerExtension>(currentContainer, this);
      this.Instance.RegisterFactory(typeof (IContainerExtension), (Func<IUnityContainer, object>) (c => (object) c.Resolve<UnityContainerExtension>(currentContainer)));
      this.Instance.RegisterType(typeof (ContainerProviderLocator), (ITypeLifetimeManager) new HierarchicalLifetimeManager());
      this.Instance.RegisterFactory(typeof (IContainerProvider), (Func<IUnityContainer, object>) (c =>
      {
        if (c.Parent == null)
          return (object) this;
        return (object) (c.Resolve<ContainerProviderLocator>().Current ?? throw new InvalidOperationException("No IContainerProvider has been set for the current Scope."));
      }));
      ExceptionExtensions.RegisterFrameworkExceptionType(typeof (ResolutionFailedException));
    }

    public IScopedProvider? CurrentScope => (IScopedProvider) this._currentScope;

    public IContainerRegistry RegisterInstance(Type type, object instance)
    {
      this.Instance.RegisterInstance(type, instance);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterInstance(Type type, object instance, string name)
    {
      this.Instance.RegisterInstance(type, name, instance);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterSingleton(Type from, Type to)
    {
      this.Instance.RegisterSingleton(from, to);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
    {
      this.Instance.RegisterSingleton(from, to, name);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
    {
      this.Instance.RegisterFactory(type, (Func<IUnityContainer, object>) (_ => factoryMethod()), (IFactoryLifetimeManager) new ContainerControlledLifetimeManager());
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterSingleton(
      Type type,
      Func<IContainerProvider, object> factoryMethod)
    {
      this.Instance.RegisterFactory(type, (Func<IUnityContainer, object>) (c => factoryMethod(c.Resolve<IContainerProvider>())), (IFactoryLifetimeManager) new ContainerControlledLifetimeManager());
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
    {
      this.Instance.RegisterSingleton(type);
      RegisterManyExtensions.GetServiceTypes(type, serviceTypes).ForEach((Action<Type>) (service => this.Instance.RegisterFactory(service, (Func<IUnityContainer, object>) (c => c.Resolve(type)), (IFactoryLifetimeManager) new ContainerControlledLifetimeManager())));
      return (IContainerRegistry) this;
    }

    public IContainerRegistry Register(Type from, Type to)
    {
      this.Instance.RegisterType(from, to);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry Register(Type from, Type to, string name)
    {
      this.Instance.RegisterType(from, to, name);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry Register(Type type, Func<object> factoryMethod)
    {
      this.Instance.RegisterFactory(type, (Func<IUnityContainer, object>) (_ => factoryMethod()));
      return (IContainerRegistry) this;
    }

    public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
    {
      this.Instance.RegisterFactory(type, (Func<IUnityContainer, object>) (c => factoryMethod(c.Resolve<IContainerProvider>())));
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
    {
      this.Instance.RegisterType(type);
      RegisterManyExtensions.GetServiceTypes(type, serviceTypes).ForEach((Action<Type>) (service => this.Instance.RegisterFactory(service, (Func<IUnityContainer, object>) (c => c.Resolve(type)))));
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterScoped(Type from, Type to)
    {
      this.Instance.RegisterType(from, to, (ITypeLifetimeManager) new HierarchicalLifetimeManager());
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
    {
      this.Instance.RegisterFactory(type, (Func<IUnityContainer, object>) (c => factoryMethod()), (IFactoryLifetimeManager) new HierarchicalLifetimeManager());
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterScoped(
      Type type,
      Func<IContainerProvider, object> factoryMethod)
    {
      this.Instance.RegisterFactory(type, (Func<IUnityContainer, object>) (c => factoryMethod(c.Resolve<IContainerProvider>())), (IFactoryLifetimeManager) new HierarchicalLifetimeManager());
      return (IContainerRegistry) this;
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
        IUnityContainer container = this._currentScope?.Container ?? this.Instance;
        DependencyOverride[] array = ((IEnumerable<(Type, object)>) parameters).Select<(Type, object), DependencyOverride>
                    ((Func<(Type, object), DependencyOverride>) (p => new DependencyOverride(p.Item1, p.Item2))).ToArray<DependencyOverride>();
        if (!typeof (IEnumerable).IsAssignableFrom(type) || type.GetGenericArguments().Length == 0)
          return container.Resolve(type, (ResolverOverride[]) array);
        type = type.GetGenericArguments()[0];
        return (object) container.ResolveAll(type, (ResolverOverride[]) array);
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
        IUnityContainer unityContainer = this._currentScope?.Container ?? this.Instance;
        if (!unityContainer.IsRegistered(type, name))
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
        return unityContainer.Resolve(type, name, (ResolverOverride[]) array);
      }
      catch (Exception ex)
      {
        throw new ContainerResolutionException(type, name, ex, (IContainerProvider) this);
      }
    }

    public bool IsRegistered(Type type) => this.Instance.IsRegistered(type);

    public bool IsRegistered(Type type, string name) => this.Instance.IsRegistered(type, name);

    Type? IContainerInfo.GetRegistrationType(string key)
    {
      return (this.Instance.Registrations.Where<IContainerRegistration>((Func<IContainerRegistration, bool>) (r => key.Equals(r.Name, StringComparison.Ordinal))).FirstOrDefault<IContainerRegistration>() ?? this.Instance.Registrations.Where<IContainerRegistration>((Func<IContainerRegistration, bool>) (r => key.Equals(r.RegisteredType.Name, StringComparison.Ordinal))).FirstOrDefault<IContainerRegistration>())?.MappedToType;
    }

    Type? IContainerInfo.GetRegistrationType(Type serviceType)
    {
      return this.Instance.Registrations.Where<IContainerRegistration>((Func<IContainerRegistration, bool>) (x => x.RegisteredType == serviceType)).FirstOrDefault<IContainerRegistration>()?.MappedToType;
    }

    public IScopedProvider CreateScope() => this.CreateScopeInternal();

    private IScopedProvider CreateScopeInternal()
    {
      this._currentScope = new UnityScopedProvider(this.Instance.CreateChildContainer());
      return (IScopedProvider) this._currentScope;
    }
  }
}
