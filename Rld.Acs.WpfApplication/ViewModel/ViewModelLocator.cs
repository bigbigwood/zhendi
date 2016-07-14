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
using Microsoft.Practices.ServiceLocation;
using Rld.Acs.WpfApplication.Navigation;
using System;

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

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AllCustomersViewModel>();
            SimpleIoc.Default.Register<SummaryPageViewModel>();
            SimpleIoc.Default.Register<DepartmentPageViewModel>();

            SimpleIoc.Default.Register<TimeSegmentPageViewModel>();
            SimpleIoc.Default.Register<TimeGroupPageViewModel>();
            SimpleIoc.Default.Register<TimeZonePageViewModel>();
        }

        private static void SetupNavigation()
        {
            SimpleIoc.Default.Unregister<IFrameNavigationService>();
            var navigationService = new FrameNavigationService();
            navigationService.Configure("UserMainWindow", new Uri("../Pages/UserMainWindow.xaml", UriKind.Relative));
            navigationService.Configure("SummaryPage", new Uri("../Pages/SummaryPage.xaml", UriKind.Relative));
            navigationService.Configure("DepartmentPage", new Uri("../Pages/DepartmentPage.xaml", UriKind.Relative));

            navigationService.Configure("TimeSegmentPage", new Uri("../Pages/TimeSegmentPage.xaml", UriKind.Relative));
            navigationService.Configure("TimeGroupPage", new Uri("../Pages/TimeGroupPage.xaml", UriKind.Relative));
            navigationService.Configure("TimeZonePage", new Uri("../Pages/TimeZonePage.xaml", UriKind.Relative));

            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public AllCustomersViewModel AllCustomersViewModel
        {
            get { return ServiceLocator.Current.GetInstance<AllCustomersViewModel>(); }
        }

        public SummaryPageViewModel SummaryPage
        {
            get { return ServiceLocator.Current.GetInstance<SummaryPageViewModel>(); }
        }

        public DepartmentPageViewModel DepartmentPage
        {
            get { return ServiceLocator.Current.GetInstance<DepartmentPageViewModel>(); }
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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}