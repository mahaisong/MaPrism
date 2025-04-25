using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Ioc.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Container.DI
{
    public class DIScopedProvider : IScopedProvider, IContainerProvider, IDisposable
    {
        private readonly List<IScopedProvider> _children = new List<IScopedProvider>();

        public IServiceProvider? Container { get; private set; }

        private IScopedProvider? _currentScope;
         
        public DIScopedProvider(IServiceProvider container)
        {
            this.Container = container;
            //做了一个子级别嵌套
            container.GetService<ContainerProviderLocator>().Current = (IContainerProvider)this;
        }

        public bool IsAttached { get; set; }
         
        public IScopedProvider? CurrentScope => _currentScope;

 
        //public IScopedProvider CreateScope() => ContainerLocator.Container.CreateScope();
        
        public IScopedProvider CreateScope() => (IScopedProvider)this;

        //嵌套3级
        public IScopedProvider CreateChildScope()
        {
            DIScopedProvider childScope = this.Container != null
                ?new DIScopedProvider(this.Container.CreateScope().ServiceProvider) : throw new InvalidOperationException("This container scope has been disposed.");
             
            this._children.Add((IScopedProvider)childScope);
            return (IScopedProvider)childScope;
        }


     
        public void Dispose()
        {
            foreach (IDisposable child in this._children)
                child.Dispose();
            this._children.Clear();
            this.Container=null; //销毁
        }

        public object Resolve(Type type)
        {
           return this.Container.GetService(type);
        }

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
                    var instances = GetServices(this.Container, elementType, parameterInstances);
                    return ConvertToTypedEnumerable(instances, elementType);
                }

                // 非集合类型解析
                return CreateInstance(this.Container, type, parameterInstances);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to resolve type {type.Name}", ex);
            }
        }

        public object Resolve(Type type, string name)
        {
            return this.Container.GetRequiredKeyedService(type,name);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            //不处理
            throw new NotImplementedException();
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
