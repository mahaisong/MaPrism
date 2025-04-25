using Prism;
using Prism.Container.DI;
using Prism.Ioc;
using ExceptionExtensions = System.ExceptionExtensions;

namespace Prism.DI
{
    /// <summary>
    /// Base application class that uses <see cref="DIContainerExtension"/> as it's container.
    /// https://docs.prismlibrary.com/docs/dependency-injection/index.html
    /// ���ڼ����ײ�����
    /// 
    /// https://prismlibrary.github.io/docs/dependency-injection/add-custom-container.html
    /// </summary>
    public abstract class PrismApplication : PrismApplicationBase
    {
        //PrismApplicationBase�����е� Initialize() �����е�һ�о��ǽ�IContainerExtension ���뵽ContainerLocator�С�
        //ContainerLocator.SetContainerExtension(CreateContainerExtension());

        /// <summary>
        /// Create a new <see cref="DIContainerExtension"/> used by Prism.
        /// </summary>
        /// <returns>A new <see cref="DIContainerExtension"/>.</returns>
        protected override IContainerExtension CreateContainerExtension()
        {
            return new DIContainerExtension();  //�����û��instance
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
