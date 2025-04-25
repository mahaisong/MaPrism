// Decompiled with JetBrains decompiler
// Type: Prism.Ioc.ContainerResolutionException
// Assembly: Prism.Container.Abstractions, Version=9.0.107.57918, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59
// MVID: 0D188376-7C8E-4737-8CFF-8151EE4E81CB
// Assembly location: C:\Users\mahai\.nuget\packages\prism.container.abstractions\9.0.107\lib\net8.0\Prism.Container.Abstractions.dll

using Prism.Ioc.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

#nullable enable
namespace Prism.Ioc
{
  public class ContainerResolutionException : Exception
  {
    public const string MissingRegistration = "No Registration was found in the container for the specified type";
    public const string CannotResolveAbstractType = "The Implementing Type is abstract.";
    public const string MultipleConstructors = "The Implementing Type has multiple constructors which may not be resolvable";
    public const string NoPublicConstructors = "The Implementing Type has no public constructors which cause issues with the type being resolved.";
    public const string CyclicalDependency = "A cyclical dependency was detected. Type A requires an instance of Type A.";
    public const string UnknownError = "You seem to have hit an edge case. Please file an issue with the Prism team along with a duplication.";

    private IContainerProvider _instance { get; }

    public ContainerResolutionException(
      Type serviceType,
      Exception innerException,
      IContainerProvider instance)
      : this(serviceType, (string) null, innerException, instance)
    {
    }

    public ContainerResolutionException(
      Type serviceType,
      string? serviceName,
      Exception innerException,
      IContainerProvider instance)
      : base(ContainerResolutionException.GetErrorMessage(serviceType, serviceName), innerException)
    {
      this._instance = instance;
      this.ServiceType = serviceType;
      this.ServiceName = serviceName;
    }

    private ContainerResolutionException(
      Type serviceType,
      string message,
      IContainerProvider instance)
      : base(message)
    {
      this._instance = instance;
      this.ServiceType = serviceType;
    }

    public Type ServiceType { get; }

    public string? ServiceName { get; }

    private bool IsKnownIssue
    {
      get
      {
        string message = this.Message;
        return message == "No Registration was found in the container for the specified type" || message == "The Implementing Type is abstract." || message == "The Implementing Type has multiple constructors which may not be resolvable" || message == "The Implementing Type has no public constructors which cause issues with the type being resolved." || message == "A cyclical dependency was detected. Type A requires an instance of Type A." || message == "You seem to have hit an edge case. Please file an issue with the Prism team along with a duplication.";
      }
    }

    public ContainerResolutionErrorCollection GetErrors()
    {
      ContainerResolutionErrorCollection errors = new ContainerResolutionErrorCollection();
      if (this.IsKnownIssue)
        return errors;
      Type type;
      if (!this.TryFindImplementingType(out type))
      {
        errors.Add(this.ServiceType, (Exception) new ContainerResolutionException(this.ServiceType, "No Registration was found in the container for the specified type", this._instance));
        return errors;
      }
      if (type.IsAbstract)
        errors.Add(this.ServiceType, (Exception) new ContainerResolutionException(type, "The Implementing Type is abstract.", this._instance));
      this.PopulateErrors(type, ref errors);
      return errors;
    }

    private bool TryFindImplementingType([MaybeNullWhen(false)] out Type type)
    {
      string serviceName = this.ServiceName;
      type = ContainerResolutionException.IsConcreteType(this.ServiceType) ? this.ServiceType : (Type) null;
      if (string.IsNullOrEmpty(serviceName))
      {
        if (!this._instance.IsRegistered(this.ServiceType))
          return (object) type != null;
        type = ContainerLocator.Current.GetRegistrationType(this.ServiceType);
      }
      else
      {
        if (serviceName != null && !this._instance.IsRegistered(this.ServiceType, serviceName) || serviceName == null)
          return (object) type != null;
        type = ContainerLocator.Current.GetRegistrationType(serviceName);
      }
      return (object) type != null;
    }

    private void PopulateErrors(
      Type implementingType,
      ref ContainerResolutionErrorCollection errors)
    {
      ConstructorInfo[] constructors = implementingType.GetConstructors();
      if (constructors.Length > 1)
        errors.Add(implementingType, (Exception) new ContainerResolutionException(implementingType, "The Implementing Type has multiple constructors which may not be resolvable", this._instance));
      else if (constructors.Length == 0)
      {
        errors.Add(implementingType, (Exception) new ContainerResolutionException(implementingType, "The Implementing Type has no public constructors which cause issues with the type being resolved.", this._instance));
        return;
      }
      ConstructorInfo constructorInfo = ((IEnumerable<ConstructorInfo>) constructors).OrderByDescending<ConstructorInfo, int>((Func<ConstructorInfo, int>) (x => x.GetParameters().Length)).FirstOrDefault<ConstructorInfo>();
      if ((object) constructorInfo == null)
      {
        errors.Add(implementingType, (Exception) new ContainerResolutionException(implementingType, "The Implementing Type has no public constructors which cause issues with the type being resolved.", this._instance));
      }
      else
      {
        ParameterInfo[] parameters = constructorInfo.GetParameters();
        List<object> objectList = new List<object>();
        foreach (ParameterInfo parameterInfo in parameters)
        {
          try
          {
            if (ContainerResolutionException.IsConcreteType(parameterInfo.ParameterType))
            {
              Type parameterType = parameterInfo.ParameterType;
            }
            if ((object) ContainerLocator.Current.GetRegistrationType(parameterInfo.ParameterType) == null)
              throw new ContainerResolutionException(parameterInfo.ParameterType, "No Registration was found in the container for the specified type", this._instance);
            object obj = this._instance.Resolve(parameterInfo.ParameterType);
            objectList.Add(obj);
          }
          catch (Exception ex)
          {
            errors.Add(parameterInfo.ParameterType, ex);
            if (ex is ContainerResolutionException resolutionException)
            {
              if (!resolutionException.IsKnownIssue)
              {
                foreach (KeyValuePair<Type, Exception> error in (IEnumerable<KeyValuePair<Type, Exception>>) resolutionException.GetErrors())
                  errors.Add(error.Key, error.Value);
              }
            }
          }
        }
        if (parameters.Length != objectList.Count)
          return;
        try
        {
          constructorInfo.Invoke(objectList.ToArray());
          throw new ContainerResolutionException(implementingType, "You seem to have hit an edge case. Please file an issue with the Prism team along with a duplication.", this._instance);
        }
        catch (TargetInvocationException ex)
        {
          errors.Add(implementingType, (Exception) ex);
          if (ex.InnerException == null)
            return;
          errors.Add(implementingType, ex.InnerException);
        }
        catch (Exception ex)
        {
          errors.Add(implementingType, ex);
        }
      }
    }

    private static bool IsConcreteType(Type type)
    {
      return !type.IsAbstract && !type.IsEnum && !type.IsPrimitive && !(type == typeof (object));
    }

    private static string GetErrorMessage(Type type, string? name)
    {
      string errorMessage = "An unexpected error occurred while resolving '" + type.FullName + "'";
      if (!string.IsNullOrEmpty(name))
        errorMessage = errorMessage + ", with the service name '" + name + "'";
      return errorMessage;
    }
  }
}
