/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Rld.Acs.WpfApplication"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.ServiceLocation;
using System;
using Rld.Acs.WpfApplication.Service.Navigation;
using Rld.Acs.WpfApplication.ViewModel.Pages;

namespace Rld.Acs.WpfApplication.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IDialogCoordinator, DialogCoordinator>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SummaryPageViewModel>();
            SimpleIoc.Default.Register<UserPageViewModel>();
            SimpleIoc.Default.Register<DepartmentPageViewModel>();
            SimpleIoc.Default.Register<DevicePageViewModel>();
            SimpleIoc.Default.Register<DeviceRolePageViewModel>();
            SimpleIoc.Default.Register<TimeSegmentPageViewModel>();
            SimpleIoc.Default.Register<TimeGroupPageViewModel>();
            SimpleIoc.Default.Register<TimeZonePageViewModel>();
            SimpleIoc.Default.Register<DeviceTrafficLogPageViewModel>();
            SimpleIoc.Default.Register<DeviceOperationLogPageViewModel>();
            SimpleIoc.Default.Register<SysDictionaryPageViewModel>();
            SimpleIoc.Default.Register<SysRolePageViewModel>();
            SimpleIoc.Default.Register<SysOperatorPageViewModel>();
            SimpleIoc.Default.Register<SysOperationLogPageViewModel>();
            SimpleIoc.Default.Register<FloorPageViewModel>();
            SimpleIoc.Default.Register<FloorMonitorPageViewModel>();
            SimpleIoc.Default.Register<DataSyncPageViewModel>();
            SimpleIoc.Default.Register<DataCleanPageViewModel>();
            SimpleIoc.Default.Register<DeviceGroupMgntViewModel>();

            SetupNavigation();
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
        }

        private static void SetupNavigation()
        {
            SimpleIoc.Default.Unregister<IFrameNavigationService>();
            var navigationService = new FrameNavigationService();
            navigationService.Configure("SummaryPage", new Uri("../Pages/SummaryPage.xaml", UriKind.Relative));
            navigationService.Configure("UserPage", new Uri("../Pages/UserPage.xaml", UriKind.Relative));
            navigationService.Configure("DepartmentPage", new Uri("../Pages/DepartmentPage.xaml", UriKind.Relative));
            navigationService.Configure("DevicePage", new Uri("../Pages/DevicePage.xaml", UriKind.Relative));
            navigationService.Configure("DeviceRolePage", new Uri("../Pages/DeviceRolePage.xaml", UriKind.Relative));
            navigationService.Configure("TimeSegmentPage", new Uri("../Pages/TimeSegmentPage.xaml", UriKind.Relative));
            navigationService.Configure("TimeGroupPage", new Uri("../Pages/TimeGroupPage.xaml", UriKind.Relative));
            navigationService.Configure("TimeZonePage", new Uri("../Pages/TimeZonePage.xaml", UriKind.Relative));
            navigationService.Configure("DeviceTrafficLogPage", new Uri("../Pages/DeviceTrafficLogPage.xaml", UriKind.Relative));
            navigationService.Configure("DeviceOperationLogPage", new Uri("../Pages/DeviceOperationLogPage.xaml", UriKind.Relative));
            navigationService.Configure("SysDictionaryPage", new Uri("../Pages/SysDictionaryPage.xaml", UriKind.Relative));
            navigationService.Configure("SysRolePage", new Uri("../Pages/SysRolePage.xaml", UriKind.Relative));
            navigationService.Configure("SysOperatorPage", new Uri("../Pages/SysOperatorPage.xaml", UriKind.Relative));
            navigationService.Configure("FloorPage", new Uri("../Pages/FloorPage.xaml", UriKind.Relative));
            navigationService.Configure("FloorMonitorPage", new Uri("../Pages/FloorMonitorPage.xaml", UriKind.Relative));
            navigationService.Configure("SysOperationLogPage", new Uri("../Pages/SysOperationLogPage.xaml", UriKind.Relative));
            navigationService.Configure("DataSyncPage", new Uri("../Pages/DataSyncPage.xaml", UriKind.Relative));
            navigationService.Configure("DataCleanPage", new Uri("../Pages/DataCleanPage.xaml", UriKind.Relative));
            
            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public SummaryPageViewModel SummaryPage
        {
            get { return ServiceLocator.Current.GetInstance<SummaryPageViewModel>(); }
        }

        public UserPageViewModel UserPage
        {
            get { return ServiceLocator.Current.GetInstance<UserPageViewModel>(); }
        }
        public DepartmentPageViewModel DepartmentPage
        {
            get { return ServiceLocator.Current.GetInstance<DepartmentPageViewModel>(); }
        }
        public DevicePageViewModel DevicePage
        {
            get { return ServiceLocator.Current.GetInstance<DevicePageViewModel>(); }
        }
        public DeviceRolePageViewModel DeviceRolePage
        {
            get { return ServiceLocator.Current.GetInstance<DeviceRolePageViewModel>(); }
        }
        public TimeSegmentPageViewModel TimeSegmentPage
        {
            get { return ServiceLocator.Current.GetInstance<TimeSegmentPageViewModel>(); }
        }
        public TimeGroupPageViewModel TimeGroupPage
        {
            get { return ServiceLocator.Current.GetInstance<TimeGroupPageViewModel>(); }
        }
        public TimeZonePageViewModel TimeZonePage
        {
            get { return ServiceLocator.Current.GetInstance<TimeZonePageViewModel>(); }
        }
        public DeviceTrafficLogPageViewModel DeviceTrafficLogPage
        {
            get { return ServiceLocator.Current.GetInstance<DeviceTrafficLogPageViewModel>(); }
        }
        public DeviceOperationLogPageViewModel DeviceOperationLogPage
        {
            get { return ServiceLocator.Current.GetInstance<DeviceOperationLogPageViewModel>(); }
        }
        public SysDictionaryPageViewModel SysDictionaryPage
        {
            get { return ServiceLocator.Current.GetInstance<SysDictionaryPageViewModel>(); }
        }
        public SysRolePageViewModel SysRolePage
        {
            get { return ServiceLocator.Current.GetInstance<SysRolePageViewModel>(); }
        }
        public SysOperatorPageViewModel SysOperatorPage
        {
            get { return ServiceLocator.Current.GetInstance<SysOperatorPageViewModel>(); }
        }
        public SysOperationLogPageViewModel SysOperationLogPage
        {
            get { return ServiceLocator.Current.GetInstance<SysOperationLogPageViewModel>(); }
        }
        public FloorPageViewModel FloorPage
        {
            get { return ServiceLocator.Current.GetInstance<FloorPageViewModel>(); }
        }
        public FloorMonitorPageViewModel FloorMonitorPage
        {
            get { return ServiceLocator.Current.GetInstance<FloorMonitorPageViewModel>(); }
        }
        public DataSyncPageViewModel DataSyncPage
        {
            get { return ServiceLocator.Current.GetInstance<DataSyncPageViewModel>(); }
        }
        public DataCleanPageViewModel DataCleanPage
        {
            get { return ServiceLocator.Current.GetInstance<DataCleanPageViewModel>(); }
        }
        public DeviceGroupMgntViewModel DeviceGroupPage
        {
            get { return ServiceLocator.Current.GetInstance<DeviceGroupMgntViewModel>(); }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}