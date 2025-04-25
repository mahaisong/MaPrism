// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.MicrosoftDependencyInjectionExtensions
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using Microsoft.Extensions.DependencyInjection;
using System;

#nullable enable
namespace Prism.Ioc
{
  public static class MicrosoftDependencyInjectionExtensions
  {
    public static void Populate(this IContainerExtension container, IServiceCollection services)
    {
      if (!(container is IServiceCollectionAware serviceCollectionAware))
        throw new InvalidOperationException("The instance of IContainerExtension does not implement IServiceCollectionAware");
      serviceCollectionAware.Populate(services);
    }

    public static IServiceProvider CreateServiceProvider(this IContainerExtension container)
    {
      return container is IServiceCollectionAware serviceCollectionAware ? serviceCollectionAware.CreateServiceProvider() : throw new InvalidOperationException("The instance of IContainerExtension does not implement IServiceCollectionAware");
    }
  }
}
