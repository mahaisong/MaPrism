// Decompiled with JetBrains decompiler
// Type: System.ExceptionHelper
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable
namespace System
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class ExceptionHelper
  {
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowIfNull<T>(T argument, [CallerArgumentExpression("argument")] string? argumentName = null)
    {
      if ((object) argument == null)
        throw new ArgumentNullException(argumentName);
    }
  }
}
