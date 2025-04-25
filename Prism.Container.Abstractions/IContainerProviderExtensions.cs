// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.IContainerProviderExtensions
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System;

#nullable enable
namespace Prism.Ioc
{
  public static class IContainerProviderExtensions
  {
    public static T Resolve<T>(this IContainerProvider provider)
    {
      return (T) provider.Resolve(typeof (T));
    }

    public static T Resolve<T>(
      this IContainerProvider provider,
      params (Type Type, object Instance)[] parameters)
    {
      return (T) provider.Resolve(typeof (T), parameters);
    }

    public static T Resolve<T>(
      this IContainerProvider provider,
      string name,
      params (Type Type, object Instance)[] parameters)
    {
      return (T) provider.Resolve(typeof (T), name, parameters);
    }

    public static T Resolve<T>(this IContainerProvider provider, string name)
    {
      return (T) provider.Resolve(typeof (T), name);
    }

    public static bool IsRegistered<T>(this IContainerProvider containerProvider)
    {
      return containerProvider.IsRegistered(typeof (T));
    }

    internal static bool IsRegistered(this IContainerProvider containerProvider, Type type)
    {
      return containerProvider is IContainerRegistry containerRegistry && containerRegistry.IsRegistered(type);
    }

    public static bool IsRegistered<T>(this IContainerProvider containerProvider, string name)
    {
      return containerProvider.IsRegistered(typeof (T), name);
    }

    internal static bool IsRegistered(
      this IContainerProvider containerProvider,
      Type type,
      string name)
    {
      return containerProvider is IContainerRegistry containerRegistry && containerRegistry.IsRegistered(type, name);
    }
  }
}
