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

        public RelayCommand InitDataCmd { get; set; }
        public RelayCommand GotoDepartmentWindowCommand { get; set; }
        public RelayCommand ShowUserMainWindow { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IFrameNavigationService navigationService)
        {
            this.navigationService = navigationService;

            InitDataCmd = new RelayCommand(InitializeData);
            GotoDepartmentWindowCommand = new RelayCommand(() => NatigateToPage("DepartmentPage"));
            ShowUserMainWindow = new RelayCommand(() => NatigateToPage("TimeSegmentPage"));
        }

        private bool simpleMenuShow = true;
        private bool allMenuShow;
        private int menuWidth = 55;

        public bool SimpleMenuShow
        {
            get
            {
                return simpleMenuShow;
            }
            set
            {
                simpleMenuShow = value;
                RaisePropertyChanged("SimpleMenuShow");
            }
        }

        public bool AllMenuShow
        {
            get
            {
                return allMenuShow;
            }
            set
            {
                allMenuShow = value;
                RaisePropertyChanged("AllMenuShow");
            }
        }

        public int MenuWidth
        {
            get
            {
                return menuWidth;
            }
            set
            {
                menuWidth = value;
                RaisePropertyChanged("MenuWidth");
             
            }
        }

        private RelayCommand<bool> showMenu;

        public RelayCommand<bool> ShowMenu
        {
            get
            {
                if(showMenu == null)
                 showMenu = new RelayCommand<bool>(b => changeMenu(b));
                return showMenu;
            }
        }

        

        private void changeMenu(bool showSimpleMenu)
        {
            if (showSimpleMenu)
            {
                SimpleMenuShow = true;
                AllMenuShow = false;
                MenuWidth = 50;
            }
            else
            {
                SimpleMenuShow = false;
                AllMenuShow = true;
                MenuWidth = 100;
                
            }
        }


        private void NatigateToPage(string pageKey)
        {
            navigationService.NavigateTo(pageKey);
        }

        private void InitializeData()
        {
            NatigateToPage("SummaryPage");
        }
    }
}