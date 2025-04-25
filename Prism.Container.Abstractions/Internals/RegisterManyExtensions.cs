// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.Internals.RegisterManyExtensions
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

#nullable enable
namespace Prism.Ioc.Internals
{
  public static class RegisterManyExtensions
  {
    private static readonly Type[] _ignoreTypes = new Type[5]
    {
      typeof (INotifyPropertyChanged),
      typeof (INotifyPropertyChanging),
      typeof (ICommand),
      typeof (IDisposable),
      typeof (IAsyncDisposable)
    };

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Type[] GetServiceTypes(Type implementingType, Type[] registrations)
    {
      IEnumerable<Type> source = ((IEnumerable<Type>) registrations).Where<Type>((Func<Type, bool>) (x => !((IEnumerable<Type>) RegisterManyExtensions._ignoreTypes).Contains<Type>(x)));
      return source.Any<Type>() ? source.ToArray<Type>() : ((IEnumerable<Type>) implementingType.GetInterfaces()).Where<Type>((Func<Type, bool>) (x => !((IEnumerable<Type>) RegisterManyExtensions._ignoreTypes).Contains<Type>(x))).ToArray<Type>();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void ForEach(this Type[] types, Action<Type> action)
    {
      for (int index = 0; index < types.Length; ++index)
        action(types[index]);
    }
  }
}
