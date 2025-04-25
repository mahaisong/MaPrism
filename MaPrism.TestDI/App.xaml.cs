using MaPrism.TestDI.Views;
using Prism.DI;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using Prism.Navigation.Regions.Behaviors;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace MaPrism.TestDI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        /// <summary>
        /// 0
        /// </summary>
        public App()
        {
            Init();
        }

        /// <summary>
        /// 0.1
        /// </summary>
        private void Init()
        {
            this.Startup += App_Startup;
        }

        /// <summary>
        /// 0.1.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
           
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            return base.CreateContainerExtension();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }


        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        /// <summary>
        /// Psrim需要的--必须优先扫描类库、DLL文件。
        /// </summary>
        /// <returns></returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return base.CreateModuleCatalog();
        }

        protected override void Initialize()
        {
            base.Initialize();

        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }

     

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
        }

     
        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            base.OnFragmentNavigation(e);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
        }

        protected override void OnNavigated(NavigationEventArgs e)
        {
            base.OnNavigated(e);
        }

        protected override void OnNavigating(NavigatingCancelEventArgs e)
        {
            base.OnNavigating(e);
        }

        protected override void OnNavigationFailed(NavigationFailedEventArgs e)
        {
            base.OnNavigationFailed(e);
        }
        protected override void OnNavigationProgress(NavigationProgressEventArgs e)
        {
            base.OnNavigationProgress(e);
        }

        protected override void OnNavigationStopped(NavigationEventArgs e)
        {
            base.OnNavigationStopped(e);
        }
        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);
        }

        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();
        }

        /// <summary>
        /// Psrim需要的--必须优先注入的各种类型
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //将 Application.Current.Dispatcher 放入DI容器中。
            //containerRegistry.Register(typeof(Dispatcher), obj => Application.Current.Dispatcher);
             
            //只有第一次的时间
            containerRegistry.RegisterSingleton<Test>();

            //每次都会是一个新的时间
            containerRegistry.Register<TestTime>();

            //注入View、导航RegisterForNavigation---对话窗口RegisterDialogWindow--对话框RegisterDialog
            //containerRegistry.RegisterForNavigation<MethodMainView>();

            //containerRegistry.Register<BindRegionContextToDependencyObjectBehavior>(BindRegionContextToDependencyObjectBehavior.BehaviorKey);
            //containerRegistry.Register<RegionActiveAwareBehavior>(RegionActiveAwareBehavior.BehaviorKey);
            //containerRegistry.Register<SyncRegionContextWithHostBehavior>(SyncRegionContextWithHostBehavior.BehaviorKey);
            //containerRegistry.Register<RegionManagerRegistrationBehavior>(RegionManagerRegistrationBehavior.BehaviorKey);
            //containerRegistry.Register<RegionMemberLifetimeBehavior>(RegionMemberLifetimeBehavior.BehaviorKey);
            //containerRegistry.Register<ClearChildViewsRegionBehavior>(ClearChildViewsRegionBehavior.BehaviorKey);
            //containerRegistry.Register<AutoPopulateRegionBehavior>(AutoPopulateRegionBehavior.BehaviorKey);
            //containerRegistry.Register<DestructibleRegionBehavior>(DestructibleRegionBehavior.BehaviorKey);
            ////containerRegistry.Register<Selector, SelectorRegionAdapter>();
            ////containerRegistry.Register<ItemsControl, ItemsControlRegionAdapter>();
            ////containerRegistry.Register<ContentControl, ContentControlRegionAdapter>();

            //containerRegistry.Register<SelectorRegionAdapter>();
            //containerRegistry.Register<ItemsControlRegionAdapter>();
            //containerRegistry.Register<ContentControlRegionAdapter>();

        }
 
         
        /// <summary>
        /// 指定入口
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>(); 
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

        /// <summary>
        /// 最后退出
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

    }

}
