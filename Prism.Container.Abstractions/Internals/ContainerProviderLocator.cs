// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.Internals.ContainerProviderLocator
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System.ComponentModel;

#nullable enable
namespace Prism.Ioc.Internals
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class ContainerProviderLocator
  {
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IContainerProvider? Current { get; set; }
  }
}
