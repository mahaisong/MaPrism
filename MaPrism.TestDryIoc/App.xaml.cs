using MaPrism.TestDryIoc.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace MaPrism.TestDryIoc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Views.MainWindow>();
        }
        /// <summary>
        /// 创建Shell后，您可能需要运行初始化步骤以确保Shell可以显示
        /// </summary>
        /// <param name="shell"></param>
        protected override void InitializeShell(Window shell)
        {
            try
            {
                var a = Container.Resolve<Views.LoginWindow>();
                var login = a?.ShowDialog();
                //5.1我们优先弹出Login窗口
                if (login == false)
                {
                    OnExit(null);
                    Environment.Exit(0);// 直接强制退出 
                }
                //5.2 如果登录成功-返回值为true了。 则进入4里面的mainWindows。

                //如果登录失败了，返回值为false了。则直接exit 退出--关闭 掉。

            }
            catch (Exception)
            {
                OnExit(null);
                Environment.Exit(0);// 直接强制退出 
            }

            base.InitializeShell(shell);
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {            //将 Application.Current.Dispatcher 放入DI容器中。
            containerRegistry.Register(typeof(Dispatcher), obj => Application.Current.Dispatcher);

            //只有第一次的时间
            containerRegistry.RegisterInstance(typeof(Test),new Test());

            //每次都会是一个新的时间
            containerRegistry.Register<TestTime>();

            //注入View、导航RegisterForNavigation---对话窗口RegisterDialogWindow--对话框RegisterDialog
            //containerRegistry.RegisterForNavigation<MethodMainView>();

            AppRomLocator.Container= (IContainerProvider)containerRegistry;
        }
    }

}
