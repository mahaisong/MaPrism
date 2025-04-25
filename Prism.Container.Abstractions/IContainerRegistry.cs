// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.IContainerRegistry
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System;

#nullable enable
namespace Prism.Ioc
{
  public interface IContainerRegistry
  {
    IContainerRegistry RegisterInstance(Type type, object instance);

    IContainerRegistry RegisterInstance(Type type, object instance, string name);

    IContainerRegistry RegisterSingleton(Type from, Type to);

    IContainerRegistry RegisterSingleton(Type from, Type to, string name);

    IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod);

    IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod);

    IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes);

    IContainerRegistry Register(Type from, Type to);

    IContainerRegistry Register(Type from, Type to, string name);

    IContainerRegistry Register(Type type, Func<object> factoryMethod);

    IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod);

    IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes);

    IContainerRegistry RegisterScoped(Type from, Type to);

    IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod);

    IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod);

    bool IsRegistered(Type type);

    bool IsRegistered(Type type, string name);
  }
}
