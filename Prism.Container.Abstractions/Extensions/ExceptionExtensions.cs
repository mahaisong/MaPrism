// Decompiled with JetBrains decompiler
// Type: System.ExceptionExtensions
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using System.Collections.Generic;

#nullable enable
namespace System
{
  public static class ExceptionExtensions
  {
    private static List<Type> frameworkExceptionTypes = new List<Type>();

    public static void RegisterFrameworkExceptionType(Type frameworkExceptionType)
    {
      if (frameworkExceptionType == (Type) null)
        throw new ArgumentNullException(nameof (frameworkExceptionType));
      if (ExceptionExtensions.frameworkExceptionTypes.Contains(frameworkExceptionType))
        return;
      ExceptionExtensions.frameworkExceptionTypes.Add(frameworkExceptionType);
    }

    public static bool IsFrameworkExceptionRegistered(Type frameworkExceptionType)
    {
      return ExceptionExtensions.frameworkExceptionTypes.Contains(frameworkExceptionType);
    }

    public static Exception GetRootException(this Exception exception)
    {
      Exception exception1 = exception;
      try
      {
        for (; exception1 != null; exception1 = exception1.InnerException)
        {
          if (!ExceptionExtensions.IsFrameworkException(exception1))
            goto label_6;
        }
        exception1 = exception;
      }
      catch (Exception ex)
      {
        exception1 = exception;
      }
label_6:
      return exception1;
    }

    private static bool IsFrameworkException(Exception exception)
    {
      int num1 = ExceptionExtensions.frameworkExceptionTypes.Contains(exception.GetType()) ? 1 : 0;
      bool flag = false;
      if (exception.InnerException != null)
        flag = ExceptionExtensions.frameworkExceptionTypes.Contains(exception.InnerException.GetType());
      int num2 = flag ? 1 : 0;
      return (num1 | num2) != 0;
    }
  }
}
