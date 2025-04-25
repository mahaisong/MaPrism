// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.ContainerLocator
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System;
using System.ComponentModel;

#nullable enable
namespace Prism.Ioc
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class ContainerLocator
  {
        /// <summary>
        /// 第二步设置也是IContainerExtension
        /// </summary>
        private static IContainerExtension? _current;

    public static bool IsInitialized => ContainerLocator._current != null;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IContainerExtension Current
    {
      get
      {
        return ContainerLocator._current ?? throw new InvalidOperationException("You must initialize the Current Container.");
      }
    }

    public static IContainerProvider Container => (IContainerProvider) ContainerLocator.Current;

        /// <summary>
        /// 第一步：进来是IContainerExtension
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainerExtension(IContainerExtension container)
    {
      ContainerLocator._current = container;
    }

    public static bool TrySetContainerExtension(IContainerExtension container)
    {
      if (ContainerLocator._current != null)
        return false;
      ContainerLocator._current = container;
      return true;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void ResetContainer() => ContainerLocator._current = (IContainerExtension) null;
  }
}
