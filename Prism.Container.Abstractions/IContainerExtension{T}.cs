// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.IContainerExtension`1
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

#nullable enable
namespace Prism.Ioc
{
  public interface IContainerExtension<TContainer> : 
    IContainerExtension,
    IContainerProvider,
    IContainerRegistry
  {
        //IContainer System.ComponentModel.IContainerd的限定类型的通用实现
        TContainer Instance { get; }
  }
}
