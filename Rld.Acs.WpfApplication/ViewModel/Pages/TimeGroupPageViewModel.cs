using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class TimeGroupPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RelayCommand AddTimeGroupCmd { get; private set; }
        public RelayCommand ModifyTimeGroupCmd { get; private set; }
        public RelayCommand DeleteTimeGroupCmd { get; private set; }

        public TimeGroupPageViewModel()
        {
            AddTimeGroupCmd = new RelayCommand(AddTimeGroup);
            ModifyTimeGroupCmd = new RelayCommand(ModifyTimeGroup);
            DeleteTimeGroupCmd = new RelayCommand(DeleteTimeGroup);
        }

        private void AddTimeGroup()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyTimeGroup()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteTimeGroup()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

    }
}
