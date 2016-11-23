using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using log4net;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class InHouseUserViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserViewModel SelectedUserViewModel { get; set; }

        public ObservableCollection<UserViewModel> UserViewModels { get; set; }
    }
}
