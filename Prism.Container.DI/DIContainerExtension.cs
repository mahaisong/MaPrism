using Prism.Ioc.Internals;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Data;
using System.Collections;

namespace Prism.Container.DI
{
    /// <summary>
    /// 核心的中转对象
    /// 1.放入对应DI容器的对象作为变量存储。
    /// 2.实现各个接口--具体实现中使用上述对象变量的方法。
    /// </summary>
    public class DIContainerExtension :
    IContainerExtension<IServiceProvider>,
    IContainerExtension,
    IContainerProvider,
    IContainerRegistry,
    IContainerInfo,
    IServiceCollectionAware
    {
        /// <summary>
        /// 注意--一定要先通过注入register后，进行BuildServiceProvider生成，才能第一次调用
        /// </summary>
        public IServiceProvider Instance => _rootProvider ??= Services.BuildServiceProvider();

        ///// <summary>
        ///// ServiceProviderOptions.Default
        ///// </summary>
        //public IServiceProvider ServiceProvider => _rootProvider ??= Services.BuildServiceProvider();


        /// <summary>
        /// 存储--IContainerRegistry--注入register使用
        /// </summary>
        private IServiceCollection Services = new ServiceCollection();


        /// <summary>
        /// 根容器，只允许ServiceProvider使用一次。 这样可以做到懒加载的效果。
        /// </summary>
        private IServiceProvider? _rootProvider;


        /// <summary>
        /// Scoped管理
        /// </summary>
        public DIScopedProvider? _currentScope;
        public IScopedProvider? CurrentScope => (IScopedProvider)this._currentScope;


        public virtual IScopedProvider CreateScope() => this.CreateScopeInternal();

        protected IScopedProvider CreateScopeInternal()
        {
            this._currentScope = new DIScopedProvider(this.Instance.CreateScope().ServiceProvider);
            return (IScopedProvider)this._currentScope;
        }

        public IServiceProvider CreateServiceProvider()
        {
            return Instance;
        }

        public Type? GetRegistrationType(string key)
        {
            throw new NotImplementedException();
        }

        public Type? GetRegistrationType(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type type)
        { 
            return Services.Any(x => x.ServiceType == type); 
        }

        public bool IsRegistered(Type type, string name)
        {
            return Services.Any(x => x.ServiceType == type&& x.IsKeyedService&& x.ServiceKey==name);
        }

        public void Populate(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 不带liftime的都按照瞬时来。Transient
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IContainerRegistry Register(Type from, Type to)
        {
            Services.AddTransient(from, to);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Services.AddKeyedTransient(from, name, to);
            return (IContainerRegistry)this;
        }

        /// <summary>
        /// 这里的factoryMethod就非常吊诡---为了做多个IOC的接口--这里非要弄成object--全部交给外部去实现
        /// </summary>
        /// <param name="type"></param>
        /// <param name="factoryMethod"></param>
        /// <returns></returns>(Func<IUnityContainer, object>) (_ => factoryMethod()));
        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            ///这个也很搞--都没有入参--每次获取这个类型的实例对象--都是通过方法---难道是随机的吗？意义不大
            Services.AddTransient(type, (Func<IServiceProvider, object>)(_ => factoryMethod()));
            return (IContainerRegistry)this;
        }

        /// <summary>
        /// 这个写法更扯--入参是IContainerProvider 其实没有意义。对应的函数内部也没有意义，因为一旦传入IContainerProvider对象，必然已经resolver了，不可能在register了。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="factoryMethod"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {

            //其实这里我可以写成invoke后再放入。  但是其内部的IContainerProvider不能提前被创建。
            throw new NotImplementedException();

            //Func<IContainerProvider, object> factoryMethod


            //Services.AddTransient(type, (Func<IServiceProvider, object>)(c => factoryMethod(c.<IContainerProvider>())));

            ////this.Instance.RegisterFactory(type, (Func<IUnityContainer, object>)(c => factoryMethod(c.Resolve<IContainerProvider>())));
            //return (IContainerRegistry)this;
        }

        /// <summary>
        /// 勉强
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            //每次获取的都是这个instance值，勉强用泛型委托包装了一下。
            Services.AddTransient(type, (Func<IServiceProvider, object>)(c => { return instance; }));
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            //每次获取的都是这个instance值，勉强用泛型委托包装了一下。
            //Func<IServiceProvider, object?, object> implementationFactory
            Services.AddKeyedTransient(type, name, (Func<IServiceProvider, object?, object>)((c, z) => { return instance; }));
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
        {
            //做多个--此写法可能并不一定对， 如果是要从resolver中解析出来，那整个方法直接返回异常即可。

            if (serviceTypes.Length == 0)
                serviceTypes = type.GetInterfaces();

            foreach (Type toserviceType in serviceTypes)
            {

                Services.AddTransient(type, toserviceType);
            }

            return (IContainerRegistry)this;


            ////不做多个，原生DI不支持resolve后再register
            //throw new NotImplementedException();

        }

        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
        {

            foreach (Type toserviceType in serviceTypes)
            {
                Services.AddSingleton(type, toserviceType);
            }

            return (IContainerRegistry)this;


            ////不做多个，原生DI不支持
            //throw new NotImplementedException();
        }

        public IContainerRegistry RegisterScoped(Type from, Type to)
        {
            Services.AddScoped(from, to);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            Services.AddScoped(type, (Func<IServiceProvider, object>)(_ => factoryMethod()));
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            //其实这里我可以写成invoke后再放入。  但是其内部的IContainerProvider不能提前被创建。
            throw new NotImplementedException();

        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Services.AddSingleton(from, to);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Services.AddKeyedSingleton(from, name, to);
            return (IContainerRegistry)this;
        }

        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            Services.AddSingleton(type, (Func<IServiceProvider, object>)(_ => factoryMethod()));
            return (IContainerRegistry)this;

        }

        public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            //其实这里我可以写成invoke后再放入。  但是其内部的IContainerProvider不能提前被创建。
            throw new NotImplementedException();
        }


        public object Resolve(Type type) => this.Resolve(type, Array.Empty<(Type, object)>());

        public object Resolve(Type type, string name)
        {
            return this.Resolve(type, name, Array.Empty<(Type, object)>());
        }
        //public object Resolve(Type type, string name)
        //{
        //    return Instance.GetRequiredKeyedService(type, name);
        //}


        //public object Resolve(Type type)
        //{
        //    return Instance.GetService(type);
        //}


        /// <summary>
        ///获取所有注册的实例--- 泛型--依赖覆盖
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            try
            {
                // 处理依赖覆盖参数
                var parameterInstances = parameters.ToDictionary(p => p.Type, p => p.Instance);

                // type一定是个IEnumerable<T> 类型
                if (typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType)
                {
                    Type elementType = type.GetGenericArguments()[0];
                    // 获取所有注册的实例
                    var instances = GetServices(Instance, elementType, parameterInstances);
                    return ConvertToTypedEnumerable(instances, elementType);
                }

                // 非集合类型解析
                return CreateInstance(Instance, type, parameterInstances);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to resolve type {type.Name}", ex);
            }
        }
         

      
        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            // 名称这里只有一对一  
            return Instance.GetRequiredKeyedService(type, name);

            //try
            //{
            //    // 处理依赖覆盖参数
            //    var parameterInstances = parameters.ToDictionary(p => p.Type, p => p.Instance);

            //    // 1. 处理带名称的解析
            //    if (!string.IsNullOrEmpty(name))
            //    {
            //        // 名称这里只有一对一 
            //        ServiceProvider.GetRequiredKeyedService(type, name); 
            //    } 
            //    // type一定是个IEnumerable<T> 类型
            //    if (typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType)
            //    {
            //        Type elementType = type.GetGenericArguments()[0];
            //        // 获取所有注册的实例
            //        var instances = GetServices(ServiceProvider, elementType, parameterInstances);
            //        return ConvertToTypedEnumerable(instances, elementType);
            //    } 
            //    // 非集合类型解析
            //    return CreateInstance(ServiceProvider, type, parameterInstances);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception($"Failed to resolve type {type.Name}", ex);
            //}

        }

        // 辅助方法：动态获取所有服务实例
        private static Array GetServices(
            IServiceProvider provider,
            Type serviceType,
            IDictionary<Type, object> parameters)
        {
            var method = typeof(ServiceProviderServiceExtensions)
                .GetMethod(nameof(ServiceProviderServiceExtensions.GetServices), new[] { typeof(IServiceProvider) })
                ?.MakeGenericMethod(serviceType);
            return (Array)method?.Invoke(null, new object[] { provider });
        }

        // 辅助方法：创建实例并覆盖依赖
        private static object CreateInstance(
            IServiceProvider provider,
            Type type,
            IDictionary<Type, object> parameters)
        {
            // 使用 ActivatorUtilities 构造实例，允许参数覆盖
            return ActivatorUtilities.CreateInstance(
                provider,
                type,
                parameters.Values.ToArray()
            );
        }

        // 辅助方法：将 Array 转换为强类型 IEnumerable<T>
        private static object ConvertToTypedEnumerable(Array array, Type elementType)
        {
            Type enumerableType = typeof(List<>).MakeGenericType(elementType);
            var typedList = Activator.CreateInstance(enumerableType);
            foreach (var item in array)
            {
                enumerableType.GetMethod("Add")?.Invoke(typedList, new[] { item });
            }
            return typedList;
        }

    }
}