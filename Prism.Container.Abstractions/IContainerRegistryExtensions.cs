// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.IContainerRegistryExtensions
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Prism.Ioc
{
  public static class IContainerRegistryExtensions
  {
    public static IContainerRegistry TryRegister(
      this IContainerRegistry containerRegistry,
      Type from,
      Type to)
    {
      if (!containerRegistry.IsRegistered(from))
        containerRegistry.Register(from, to);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegister<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TTo>(
      this IContainerRegistry containerRegistry)
      where TTo : TFrom
    {
      return containerRegistry.TryRegister(typeof (TFrom), typeof (TTo));
    }

    public static IContainerRegistry TryRegister<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry)
      where T : class
    {
      return containerRegistry.TryRegister(typeof (T), typeof (T));
    }

    public static IContainerRegistry TryRegister<T>(
      this IContainerRegistry containerRegistry,
      Func<IContainerProvider, object> factoryMethod)
    {
      if (!containerRegistry.IsRegistered<T>())
        containerRegistry.Register<T>(factoryMethod);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegister<T>(
      this IContainerRegistry containerRegistry,
      Func<object> factoryMethod)
    {
      if (!containerRegistry.IsRegistered<T>())
        containerRegistry.Register<T>(factoryMethod);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegisterScoped(
      this IContainerRegistry containerRegistry,
      Type from,
      Type to)
    {
      if (!containerRegistry.IsRegistered(from))
        containerRegistry.RegisterScoped(from, to);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegisterScoped<T>(this IContainerRegistry containerRegistry) where T : class
    {
      return containerRegistry.TryRegisterScoped(typeof (T), typeof (T));
    }

    public static IContainerRegistry TryRegisterScoped<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TTo>(
      this IContainerRegistry containerRegistry)
      where TTo : TFrom
    {
      return containerRegistry.TryRegisterScoped(typeof (TFrom), typeof (TTo));
    }

    public static IContainerRegistry TryRegisterScoped<T>(
      this IContainerRegistry containerRegistry,
      Func<IContainerProvider, object> factoryMethod)
    {
      if (!containerRegistry.IsRegistered<T>())
        containerRegistry.RegisterScoped<T>(factoryMethod);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegisterScoped<T>(
      this IContainerRegistry containerRegistry,
      Func<object> factoryMethod)
    {
      if (!containerRegistry.IsRegistered<T>())
        containerRegistry.RegisterScoped<T>(factoryMethod);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegisterSingleton(
      this IContainerRegistry containerRegistry,
      Type from,
      Type to)
    {
      if (!containerRegistry.IsRegistered(from))
        containerRegistry.RegisterSingleton(from, to);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegisterSingleton<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TTo>(
      this IContainerRegistry containerRegistry)
      where TTo : TFrom
    {
      return containerRegistry.TryRegisterSingleton(typeof (TFrom), typeof (TTo));
    }

    public static IContainerRegistry TryRegisterSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry)
      where T : class
    {
      return containerRegistry.TryRegisterSingleton(typeof (T), typeof (T));
    }

    public static IContainerRegistry TryRegisterInstance<T>(
      this IContainerRegistry containerRegistry,
      T instance)
    {
      if (!containerRegistry.IsRegistered<T>())
        containerRegistry.RegisterInstance<T>(instance);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegisterSingleton<T>(
      this IContainerRegistry containerRegistry,
      Func<IContainerProvider, object> factoryMethod)
    {
      if (!containerRegistry.IsRegistered<T>())
        containerRegistry.RegisterSingleton<T>(factoryMethod);
      return containerRegistry;
    }

    public static IContainerRegistry TryRegisterSingleton<T>(
      this IContainerRegistry containerRegistry,
      Func<object> factoryMethod)
    {
      if (!containerRegistry.IsRegistered<T>())
        containerRegistry.RegisterSingleton<T>(factoryMethod);
      return containerRegistry;
    }

    public static IContainerRegistry RegisterInstance<TInterface>(
      this IContainerRegistry containerRegistry,
      TInterface instance)
    {
      return (object) instance != null ? containerRegistry.RegisterInstance(typeof (TInterface), (object) instance) : throw new ArgumentNullException(nameof (instance));
    }

    public static IContainerRegistry RegisterInstance<TInterface>(
      this IContainerRegistry containerRegistry,
      TInterface instance,
      string name)
    {
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      return containerRegistry.RegisterInstance(typeof (TInterface), (object) instance, name);
    }

    public static IContainerRegistry RegisterSingleton(
      this IContainerRegistry containerRegistry,
      Type type)
    {
      return containerRegistry.RegisterSingleton(type, type);
    }

    public static IContainerRegistry RegisterSingleton<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TTo>(
      this IContainerRegistry containerRegistry)
      where TTo : TFrom
    {
      return containerRegistry.RegisterSingleton(typeof (TFrom), typeof (TTo));
    }

    public static IContainerRegistry RegisterSingleton<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TTo>(
      this IContainerRegistry containerRegistry,
      string name)
      where TTo : TFrom
    {
      return containerRegistry.RegisterSingleton(typeof (TFrom), typeof (TTo), name);
    }

    public static IContainerRegistry RegisterSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry)
    {
      return containerRegistry.RegisterSingleton(typeof (T));
    }

    public static IContainerRegistry RegisterSingleton<T>(
      this IContainerRegistry containerRegistry,
      Func<object> factoryMethod)
    {
      return containerRegistry.RegisterSingleton(typeof (T), factoryMethod);
    }

    public static IContainerRegistry RegisterSingleton<T>(
      this IContainerRegistry containerRegistry,
      Func<IContainerProvider, object> factoryMethod)
    {
      return containerRegistry.RegisterSingleton(typeof (T), factoryMethod);
    }

    public static IContainerRegistry RegisterManySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry,
      params Type[] serviceTypes)
    {
      return containerRegistry.RegisterManySingleton(typeof (T), serviceTypes);
    }

    public static IContainerRegistry Register(this IContainerRegistry containerRegistry, Type type)
    {
      return containerRegistry.Register(type, type);
    }

    public static IContainerRegistry Register<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry)
    {
      return containerRegistry.Register(typeof (T));
    }

    public static IContainerRegistry Register(
      this IContainerRegistry containerRegistry,
      Type type,
      string name)
    {
      return containerRegistry.Register(type, type, name);
    }

    public static IContainerRegistry Register<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry,
      string name)
    {
      return containerRegistry.Register(typeof (T), name);
    }

    public static IContainerRegistry Register<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TTo>(
      this IContainerRegistry containerRegistry)
      where TTo : TFrom
    {
      return containerRegistry.Register(typeof (TFrom), typeof (TTo));
    }

    public static IContainerRegistry Register<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TTo>(
      this IContainerRegistry containerRegistry,
      string name)
      where TTo : TFrom
    {
      return containerRegistry.Register(typeof (TFrom), typeof (TTo), name);
    }

    public static IContainerRegistry Register<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry,
      Func<object> factoryMethod)
    {
      return containerRegistry.Register(typeof (T), factoryMethod);
    }

    public static IContainerRegistry Register<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry,
      Func<IContainerProvider, object> factoryMethod)
    {
      return containerRegistry.Register(typeof (T), factoryMethod);
    }

    public static IContainerRegistry RegisterMany<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry,
      params Type[] serviceTypes)
    {
      return containerRegistry.RegisterMany(typeof (T), serviceTypes);
    }

    public static IContainerRegistry RegisterScoped(
      this IContainerRegistry containerRegistry,
      Type type)
    {
      return containerRegistry.RegisterScoped(type, type);
    }

    public static IContainerRegistry RegisterScoped<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
      this IContainerRegistry containerRegistry)
    {
      return containerRegistry.RegisterScoped(typeof (T));
    }

    public static IContainerRegistry RegisterScoped<TFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] TTo>(
      this IContainerRegistry containerRegistry)
      where TTo : TFrom
    {
      return containerRegistry.RegisterScoped(typeof (TFrom), typeof (TTo));
    }

    public static IContainerRegistry RegisterScoped<T>(
      this IContainerRegistry containerRegistry,
      Func<object> factoryMethod)
    {
      return containerRegistry.RegisterScoped(typeof (T), factoryMethod);
    }

    public static IContainerRegistry RegisterScoped<T>(
      this IContainerRegistry containerRegistry,
      Func<IContainerProvider, object> factoryMethod)
    {
      return containerRegistry.RegisterScoped(typeof (T), factoryMethod);
    }

    public static bool IsRegistered<T>(this IContainerRegistry containerRegistry)
    {
      return containerRegistry.IsRegistered(typeof (T));
    }

    public static bool IsRegistered<T>(this IContainerRegistry containerRegistry, string name)
    {
      return containerRegistry.IsRegistered(typeof (T), name);
    }
  }
}
