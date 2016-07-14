using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Rld.Acs.WpfApplication.Navigation;

namespace Rld.Acs.WpfApplication.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IFrameNavigationService navigationService;
        public RelayCommand<string> NatigatePageCmd { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IFrameNavigationService navigationService)
        {
            this.navigationService = navigationService;
            NatigatePageCmd = new RelayCommand<string>(NatigateToPage);
        }

        private void NatigateToPage(string pageKey)
        {
            navigationService.NavigateTo(pageKey);
        }
    }
}