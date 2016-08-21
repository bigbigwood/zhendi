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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}