// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.ContainerResolutionErrorCollection
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Prism.Ioc
{
  public sealed class ContainerResolutionErrorCollection : 
    IEnumerable<KeyValuePair<Type, Exception>>,
    IEnumerable
  {
    private readonly List<KeyValuePair<Type, Exception>> _errors = new List<KeyValuePair<Type, Exception>>();

    internal void Add(Type type, Exception exception)
    {
      this._errors.Add(new KeyValuePair<Type, Exception>(type, exception));
    }

    public IEnumerable<Type> Types
    {
      get
      {
        return this._errors.Select<KeyValuePair<Type, Exception>, Type>((Func<KeyValuePair<Type, Exception>, Type>) (x => x.Key)).Distinct<Type>();
      }
    }

    IEnumerator<KeyValuePair<Type, Exception>> IEnumerable<KeyValuePair<Type, Exception>>.GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<Type, Exception>>) this._errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._errors.GetEnumerator();
  }
}
