

using Prism.Container.DI;
using Prism.Ioc;

namespace Prism.DI
{
    /// <summary>
    /// Base bootstrapper class that uses <see cref="DIContainerExtension"/> as it's container.
    /// 
    /// https://docs.prismlibrary.com/docs/dependency-injection/index.html
    /// IContainerExtension 的实现
    /// 扩展类：指的是DIContainerExtension
    /// </summary>
    public abstract class PrismBootstrapper : PrismBootstrapperBase
    {

        /// <summary>
        /// Create a new <see cref="DIContainerExtension"/> used by Prism.
        /// </summary>
        /// <returns>A new <see cref="DIContainerExtension"/>.</returns>
        protected override IContainerExtension CreateContainerExtension()
        {
            return new DIContainerExtension();
        }

        /// <summary>
        /// Registers the <see cref="Type"/>s of the Exceptions that are not considered 
        /// root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected override void RegisterFrameworkExceptionTypes()
        {
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(InvalidOperationException));
        }
    }
}
