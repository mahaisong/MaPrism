using DryIoc;
using Prism.Ioc;
using Prism.Container.DryIoc;

#nullable disable
namespace Prism.DryIoc
{
    /// <summary>
    /// Base application class that uses <see cref="T:Prism.Container.DryIoc.DryIocContainerExtension" /> as it's container.
    /// </summary>
    public abstract class PrismApplication : PrismApplicationBase
    {
        /// <summary>
        /// Create <see cref="T:DryIoc.Rules" /> to alter behavior of <see cref="T:DryIoc.IContainer" />
        /// </summary>
        /// <returns>An instance of <see cref="T:DryIoc.Rules" /></returns>
        protected virtual Rules CreateContainerRules() => DryIocContainerExtension.DefaultRules;

        /// <summary>
        /// Create a new <see cref="T:Prism.Container.DryIoc.DryIocContainerExtension" /> used by Prism.
        /// </summary>
        /// <returns>A new <see cref="T:Prism.Container.DryIoc.DryIocContainerExtension" />.</returns>
        protected override IContainerExtension CreateContainerExtension()
        {
            return (IContainerExtension)new DryIocContainerExtension(this.CreateContainerRules());
        }

        /// <summary>
        /// Registers the <see cref="T:System.Type" />s of the Exceptions that are not considered
        /// root exceptions by the <see cref="T:System.ExceptionExtensions" />.
        /// </summary>
        protected virtual void RegisterFrameworkExceptionTypes()
        {
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ContainerException));
        }
    }
}
