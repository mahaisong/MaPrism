 
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Ioc.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DIoc = DryIoc;

#nullable enable
namespace Prism.Container.DryIoc
{
    public class DryIocContainerExtension :
      IContainerExtension<IContainer>,
      IContainerExtension,
      IContainerProvider,
      IContainerRegistry,
      IContainerInfo,
      IServiceCollectionAware
    {
        private DryIocScopedProvider? _currentScope;

        private static Rules GetDefaultRules()
        {
            Rules defaultRules = Rules.Default.WithConcreteTypeDynamicRegistrations((Func<Type, object, bool>)null, Reuse.Transient).With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments, (ParameterSelector)null, (PropertiesAndFieldsSelector)null, false), false).WithFuncAndLazyWithoutRegistration().WithTrackingDisposableTransients().WithFactorySelector(Rules.SelectLastRegisteredFactory());
            try
            {
                AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("PrismDynamicAssembly"), AssemblyBuilderAccess.Run).DefineDynamicModule("DynamicModule");
                return defaultRules;
            }
            catch
            {
                return defaultRules.WithUseInterpretation();
            }
        }

        public static Rules DefaultRules { get; } = DryIocContainerExtension.GetDefaultRules();

        public IContainer Instance { get; }

        public DryIocContainerExtension()
          : this(DryIocContainerExtension.DefaultRules)
        {
        }

        public DryIocContainerExtension(Rules rules)
          : this((IContainer)new DIoc.Container(rules, (IScopeContext)null))
        {
        }

        public DryIocContainerExtension(IContainer container)
        {
            this.Instance = container;
            Registrator.RegisterInstance<IContainerExtension>((IRegistrator)this.Instance, (IContainerExtension)this, new IfAlreadyRegistered?(), (Setup)null, (object)null);
            Registrator.Register<ContainerProviderLocator>((IRegistrator)this.Instance, Reuse.Scoped, (Made)null, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            Registrator.RegisterDelegate<IContainerProvider>((IRegistrator)this.Instance, (Func<IResolverContext, IContainerProvider>)(r =>
            {
                if (!ResolverContext.IsScoped(r))
                    return (IContainerProvider)this;
                return Resolver.Resolve<ContainerProviderLocator>((IResolver)r, (IfUnresolved)0).Current ?? throw new InvalidOperationException("The Container has not been set for this Scope.");
            }), Reuse.Transient, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ContainerException));
        }

        public IScopedProvider? CurrentScope => (IScopedProvider)this._currentScope;

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Registrator.RegisterInstance((IRegistrator)this.Instance, type, instance, new IfAlreadyRegistered?(), (Setup)null, (object)null);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Registrator.RegisterInstance((IRegistrator)this.Instance, type, instance, new IfAlreadyRegistered?((IfAlreadyRegistered)3), (Setup)null, (object)name);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Registrator.Register((IRegistrator)this.Instance, from, to, Reuse.Singleton);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Registrator.Register((IRegistrator)this.Instance, from, to, Reuse.Singleton, (Made)null, (Setup)null, new IfAlreadyRegistered?((IfAlreadyRegistered)3), (object)name);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            Registrator.RegisterDelegate((IRegistrator)this.Instance, type, (Func<IResolverContext, object>)(r => factoryMethod()), Reuse.Singleton, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterSingleton(
          Type type,
          Func<IContainerProvider, object> factoryMethod)
        {
            Registrator.RegisterDelegate<IContainerProvider>((IRegistrator)this.Instance, type, factoryMethod, Reuse.Singleton, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
        {
            if (serviceTypes.Length == 0)
                serviceTypes = type.GetInterfaces();
            Registrator.RegisterMany((IRegistrator)this.Instance, serviceTypes, type, Reuse.Singleton, (Made)null, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterScoped(Type from, Type to)
        {
            Registrator.Register((IRegistrator)this.Instance, from, to, Reuse.ScopedOrSingleton);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            Registrator.RegisterDelegate((IRegistrator)this.Instance, type, (Func<IResolverContext, object>)(r => factoryMethod()), Reuse.ScopedOrSingleton, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterScoped(
          Type type,
          Func<IContainerProvider, object> factoryMethod)
        {
            Registrator.RegisterDelegate<IContainerProvider>((IRegistrator)this.Instance, type, factoryMethod, Reuse.ScopedOrSingleton, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Registrator.Register((IRegistrator)this.Instance, from, to, Reuse.Transient);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Registrator.Register((IRegistrator)this.Instance, from, to, Reuse.Transient, (Made)null, (Setup)null, new IfAlreadyRegistered?((IfAlreadyRegistered)3), (object)name);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            Registrator.RegisterDelegate((IRegistrator)this.Instance, type, (Func<IResolverContext, object>)(r => factoryMethod()), Reuse.Transient, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            Registrator.RegisterDelegate<IContainerProvider>((IRegistrator)this.Instance, type, factoryMethod, Reuse.Transient, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
        {
            if (serviceTypes.Length == 0)
                serviceTypes = type.GetInterfaces();
            Registrator.RegisterMany((IRegistrator)this.Instance, serviceTypes, type, Reuse.Transient, (Made)null, (Setup)null, new IfAlreadyRegistered?(), (object)null);
            return (IContainerRegistry)this;
        }

        public object Resolve(Type type) => this.Resolve(type, Array.Empty<(Type, object)>());

        public object Resolve(Type type, string name)
        {
            return this.Resolve(type, name, Array.Empty<(Type, object)>());
        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            try
            {
                List<object> list = ((IEnumerable<(Type, object)>)parameters).Where<(Type, object)>((Func<(Type, object), bool>)(x => !(x is IContainerProvider))).Select<(Type, object), object>((Func<(Type, object), object>)(x => x)).ToList<object>();
                list.Add((object)this);
                return Resolver.Resolve((IResolver)this.Instance, type, list.ToArray(), (IfUnresolved)0, (Type)null, (object)null);
            }
            catch (Exception ex)
            {
                throw new ContainerResolutionException(type, ex, (IContainerProvider)this);
            }
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            try
            {
                List<object> list = ((IEnumerable<(Type, object)>)parameters).Where<(Type, object)>((Func<(Type, object), bool>)(x => !(x is IContainerProvider))).Select<(Type, object), object>((Func<(Type, object), object>)(x => x)).ToList<object>();
                list.Add((object)this);
                return Resolver.Resolve((IResolver)this.Instance, type, (object)name, (IfUnresolved)0, (Type)null, list.ToArray());
            }
            catch (Exception ex)
            {
                throw new ContainerResolutionException(type, name, ex, (IContainerProvider)this);
            }
        }

        public bool IsRegistered(Type type)
        {
            return Registrator.IsRegistered((IRegistrator)this.Instance, type, (object)null, (FactoryType)0, (Func<Factory, bool>)null);
        }

        public bool IsRegistered(Type type, string name)
        {
            return Registrator.IsRegistered((IRegistrator)this.Instance, type, (object)name, (FactoryType)0, (Func<Factory, bool>)null) || Registrator.IsRegistered((IRegistrator)this.Instance, type, (object)name, (FactoryType)2, (Func<Factory, bool>)null);
        }

        Type? IContainerInfo.GetRegistrationType(string key)
        {
            ServiceRegistrationInfo registrationInfo = ((IRegistrator)this.Instance).GetServiceRegistrations().Where<ServiceRegistrationInfo>((Func<ServiceRegistrationInfo, bool>)(r => key.Equals(r.OptionalServiceKey?.ToString(), StringComparison.Ordinal))).FirstOrDefault<ServiceRegistrationInfo>();
            if (registrationInfo.OptionalServiceKey == null)
                registrationInfo = ((IRegistrator)this.Instance).GetServiceRegistrations().Where<ServiceRegistrationInfo>((Func<ServiceRegistrationInfo, bool>)(r => key.Equals(((ServiceRegistrationInfo)r).ImplementationType.Name, StringComparison.Ordinal))).FirstOrDefault<ServiceRegistrationInfo>();
            return ((ServiceRegistrationInfo) registrationInfo).ImplementationType;
        }

        Type? IContainerInfo.GetRegistrationType(Type serviceType)
        {
            ServiceRegistrationInfo registrationInfo = ((IRegistrator)this.Instance).GetServiceRegistrations().Where<ServiceRegistrationInfo>((Func<ServiceRegistrationInfo, bool>)(x => x.ServiceType == serviceType)).FirstOrDefault<ServiceRegistrationInfo>();
            return (object)registrationInfo.ServiceType != null ? ((ServiceRegistrationInfo) registrationInfo).ImplementationType : (Type)null;
        }

        public virtual IScopedProvider CreateScope() => this.CreateScopeInternal();

        protected IScopedProvider CreateScopeInternal()
        {
            this._currentScope = new DryIocScopedProvider(ResolverContext.OpenScope((IResolverContext)this.Instance));
            return (IScopedProvider)this._currentScope;
        }

        public IServiceProvider CreateServiceProvider()
        {
            DryIocServiceProviderCapabilities providerCapabilities = new DryIocServiceProviderCapabilities(this.Instance);
            IScope singletonScope = ((IResolverContext)this.Instance).SingletonScope;
            ScopeTools.Use<IServiceProviderIsService>(singletonScope, (object)providerCapabilities);
            ScopeTools.Use<ISupportRequiredService>(singletonScope, (object)providerCapabilities);
            //改造lambda
            ScopeTools.UseFactory<IServiceScopeFactory>(singletonScope, (x) => { return new DryIocServiceScopeFactory(x) ; });
             
            return (IServiceProvider)this.Instance;
        }

        public void Populate(IServiceCollection services)
        {
            foreach (ServiceDescriptor service in (IEnumerable<ServiceDescriptor>)services)
                DryIocContainerExtension.RegisterDescriptor(this.Instance, service);
            ContainerTools.Validate(this.Instance, (Func<ServiceRegistrationInfo, bool>)null);
        }

        private static IReuse ToReuse(ServiceLifetime lifetime)
        {
            if (lifetime == ServiceLifetime.Singleton)
                return Reuse.Singleton;
            return lifetime != ServiceLifetime.Scoped ? Reuse.Transient : Reuse.ScopedOrSingleton;
        }

        private static void RegisterDescriptor(IContainer container, ServiceDescriptor descriptor)
        {
            ExceptionHelper.ThrowIfNull<Type>(descriptor.ServiceType, "descriptor.ServiceType");
            Type serviceType = descriptor.ServiceType;
            IReuse reuse = DryIocContainerExtension.ToReuse(descriptor.Lifetime);
            if (descriptor.IsKeyedService)
            {
                ExceptionHelper.ThrowIfNull<object>(descriptor.ServiceKey, "descriptor.ServiceKey");
                DryIocContainerExtension.RegisterKeyedDescriptor(container, serviceType, descriptor, reuse, descriptor.ServiceKey);
            }
            else
                DryIocContainerExtension.RegisterDescriptor(container, serviceType, descriptor, reuse);
        }

        private static void RegisterKeyedDescriptor(
          IContainer container,
          Type serviceType,
          ServiceDescriptor descriptor,
          IReuse reuse,
          object? key)
        {
            Type implementationType = descriptor.KeyedImplementationType;
            if (implementationType != (Type)null)
                ((IRegistrator)container).Register((Factory)ReflectionFactory.Of(implementationType, reuse), serviceType, key, new IfAlreadyRegistered?(), implementationType == serviceType);
            else if (descriptor.KeyedImplementationFactory != null)
            {
                 
                ((IRegistrator)container).Register(
                  (Factory)DelegateFactory.Of(x=> descriptor.KeyedImplementationFactory), 
                 serviceType, key, new IfAlreadyRegistered?(), true);

                //// ISSUE: method pointer
                //((IRegistrator)container).Register(
                //    (Factory)DelegateFactory.Of//  FactoryDelegateCompiler.CompileOrInterpretFactoryDelegate(
                //        (object)descriptor.KeyedImplementationFactory)
                //    (
                //        new FactoryDelegate(
                //        (object)descriptor.KeyedImplementationFactory, 
                //        __methodptr(ToFactoryDelegate))
                //    , reuse), serviceType, key, new IfAlreadyRegistered?(), true);
            }
            else
            {
                object implementationInstance = descriptor.ImplementationInstance;
                ((IRegistrator)container).Register((Factory)InstanceFactory.Of(implementationInstance), serviceType, key, new IfAlreadyRegistered?(), true);
                Registrator.TrackDisposable((IRegistrator)container, implementationInstance);
            }
        }

        private static void RegisterDescriptor(
          IContainer container,
          Type serviceType,
          ServiceDescriptor descriptor,
          IReuse reuse)
        {
            Type implementationType = descriptor.ImplementationType;
            if (implementationType != (Type)null)
                ((IRegistrator)container).Register((Factory)ReflectionFactory.Of(implementationType, reuse), serviceType, (object)null, new IfAlreadyRegistered?(), implementationType == serviceType);
            else if (descriptor.ImplementationFactory != null)
            {
                ((IRegistrator)container).Register((Factory)DelegateFactory.Of(x=>descriptor.ImplementationFactory), serviceType, (object)null, new IfAlreadyRegistered?(), true);


                //// ISSUE: method pointer
                //((IRegistrator)container).Register((Factory)DelegateFactory.Of(
                //    new FactoryDelegate((object)descriptor.ImplementationFactory,
                //    __methodptr(ToFactoryDelegate<object>)), reuse), serviceType, (object)null, new IfAlreadyRegistered?(), true);
            }
            else
            {
                object implementationInstance = descriptor.ImplementationInstance;
                ((IRegistrator)container).Register((Factory)InstanceFactory.Of(implementationInstance), serviceType, (object)null, new IfAlreadyRegistered?(), true);
                Registrator.TrackDisposable((IRegistrator)container, implementationInstance);
            }
        }
    }
}
