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
    public class TimeZonePageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RelayCommand AddTimeZoneCmd { get; private set; }
        public RelayCommand ModifyTimeZoneCmd { get; private set; }
        public RelayCommand DeleteTimeZoneCmd { get; private set; }

        public TimeZonePageViewModel()
        {
            AddTimeZoneCmd = new RelayCommand(AddTimeZone);
            ModifyTimeZoneCmd = new RelayCommand(ModifyTimeZone);
            DeleteTimeZoneCmd = new RelayCommand(DeleteTimeZone);
        }

        private void AddTimeZone()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyTimeZone()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteTimeZone()
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
