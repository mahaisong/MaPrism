// Decompiled with JetBrains decompiler
// Type: Prism.Container.DryIoc.DryIocScopedProvider
// Assembly: Prism.Container.DryIoc, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: EEDC524D-D565-4B74-821B-379775634F5C
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.dryioc\9.0.107\lib\net8.0\Prism.Container.DryIoc.dll

using DryIoc;
using Prism.Ioc;
using Prism.Ioc.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using DIoc = DryIoc;

#nullable enable
namespace Prism.Container.DryIoc
{
  internal class DryIocScopedProvider : IScopedProvider, IContainerProvider, IDisposable
  {
    private readonly List<IScopedProvider> _children = new List<IScopedProvider>();

    public DryIocScopedProvider(IResolverContext resolver)
    {
      this.Resolver = resolver;
            DIoc.Resolver.Resolve<ContainerProviderLocator>((IResolver) this.Resolver, (IfUnresolved) 0).Current = (IContainerProvider) this;
    }

    public bool IsAttached { get; set; }

    public IResolverContext? Resolver { get; private set; }

    public IScopedProvider? CurrentScope => (IScopedProvider) this;

    public IScopedProvider CreateChildScope()
    {
      DryIocScopedProvider childScope = new DryIocScopedProvider(ResolverContext.OpenScope(this.Resolver));
      this._children.Add((IScopedProvider) childScope);
      return (IScopedProvider) childScope;
    }

    public IScopedProvider CreateScope() => ContainerLocator.Container.CreateScope();

    public void Dispose()
    {
      this._children.ForEach((Action<IScopedProvider>) (x => x.Dispose()));
      this._children.Clear();
      ((IDisposable) this.Resolver)?.Dispose();
      this.Resolver = (IResolverContext) null;
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
                List<object> list = (from x in parameters
                                     where !(x.Instance is IContainerProvider) //去掉嵌入DI自身的对象--因为ioc不是微软DI-微软DI不允许嵌入自身。但是其他DI都允许
                                     select x.Instance).ToList();
                //改写法，直接用linq to object 
                //List<object> list =
                //    ((IEnumerable<(Type, object)>)parameters).//转IEnumerable
                //    Where<(Type, object)>(
                //        (Func<(Type, object), bool>)
                //    (x => !(x is IContainerProvider))
                //    )//where语句
                //    .Select<(Type, object), object>(
                //        (Func<(Type, object), object>)
                //        (x => x)
                //        ).ToList<object>();
                list.Add((object) this);
        return DIoc.Resolver.Resolve((IResolver) this.Resolver, type, list.ToArray(), (IfUnresolved) 0, (Type) null, (object) null);
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
        List<object> list = ((IEnumerable<(Type, object)>) parameters).Where<(Type, object)>((Func<(Type, object), bool>) (x => !(x is IContainerProvider))).Select<(Type, object), object>((Func<(Type, object), object>) (x => x)).ToList<object>();
        list.Add((object) this);
        return DIoc.Resolver.Resolve((IResolver) this.Resolver, type, (object) name, (IfUnresolved) 0, (Type) null, list.ToArray());
      }
      catch (Exception ex)
      {
        throw new ContainerResolutionException(type, name, ex, (IContainerProvider) this);
      }
    }
  }
}
