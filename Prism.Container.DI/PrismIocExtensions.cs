using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Container.DI
{

    //直接注释掉--防止被误导

    /// <summary>
    /// 得到Provider的静态工具类--得到对象的Instance示例
    /// https://docs.prismlibrary.com/docs/dependency-injection/index.html
    /// 
    /// IContainerProvider的扩展方法
    /// IContainerRegistry的扩展方法
    /// https://prismlibrary.github.io/docs/dependency-injection/add-custom-container.html
    /// 
    /// </summary>
    public static class PrismIocExtensions
    {
        /// <summary>
        /// IContainer是一种通用实现--对照的是DIContainerExtension。
        /// </summary>
        /// <param name="containerProvider"></param>
        /// <returns></returns>
        public static IServiceProvider GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<IServiceProvider>)containerProvider).Instance;
        }

        /// <summary>
        /// 这里IServiceCollection
        /// </summary>
        /// <param name="containerRegistry"></param>
        /// <returns></returns>
        public static IServiceProvider GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<IServiceProvider>)containerRegistry).Instance;
        }
    }
}
