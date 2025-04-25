// Decompiled with JetBrains decompiler
// Type: Prism.Container.Unity.PrismIocExtensions
// Assembly: Prism.Container.Unity, Version=9.0.114.15915, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: E01EFCD6-DCE7-4021-B1F2-2FA2B4C29C0F
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.unity\9.0.114\lib\net8.0\Prism.Container.Unity.dll

using Prism.Ioc;
using Unity;

#nullable enable
namespace Prism.Container.Unity
{
  public static class PrismIocExtensions
  {
    public static IUnityContainer GetContainer(this IContainerProvider containerProvider)
    {
      return ((IContainerExtension<IUnityContainer>) containerProvider).Instance;
    }

    public static IUnityContainer GetContainer(this IContainerRegistry containerRegistry)
    {
      return ((IContainerExtension<IUnityContainer>) containerRegistry).Instance;
    }
  }
}
