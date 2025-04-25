using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaPrism.TestDryIocAddInDI.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private IRegionManager _regionManager;
        public LoginWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("TestRegion", typeof(TestRegionView));

        }
    }
}
