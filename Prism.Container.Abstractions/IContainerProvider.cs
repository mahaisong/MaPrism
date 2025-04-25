// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.IContainerProvider
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System;

#nullable enable
namespace Prism.Ioc
{
  public interface IContainerProvider
  {
    object Resolve(Type type);

    object Resolve(Type type, params (Type Type, object Instance)[] parameters);

    object Resolve(Type type, string name);

    object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters);

    IScopedProvider CreateScope();

    IScopedProvider? CurrentScope { get; }
  }
}
