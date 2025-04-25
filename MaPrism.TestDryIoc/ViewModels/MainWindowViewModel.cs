using Prism.Commands;
using Prism.Dialogs;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaPrism.TestDryIoc.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {


        public ICommand TestCommand { get; set; }

        #region 构造函数参数
        private IDialogService _dialogService;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private IContainerProvider _containerProvider;

        private Test test;
        private TestTime testTime;
        #endregion
        /// <summary>
        /// DI中在构造函数中立即使用的，请写入参数
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="regionManager"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="MessagingService"></param>
        public MainWindowViewModel(IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator,
            IContainerProvider containerProvider, Test _test, TestTime _testTime)
        {
            _dialogService = dialogService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            _containerProvider = containerProvider;
            test = _test;
            testTime = _testTime;
            TestCommand = new DelegateCommand(Do);
        }

        private void Do()
        { 
            Debug.WriteLine("Main______");
            Debug.WriteLine(test.DateTime);
            Debug.WriteLine(testTime.DateTime);
            Debug.WriteLine(AppRomLocator.Container.Resolve<Test>().DateTime);
            Debug.WriteLine(AppRomLocator.Container.Resolve<TestTime>().DateTime);

        }
    }
}