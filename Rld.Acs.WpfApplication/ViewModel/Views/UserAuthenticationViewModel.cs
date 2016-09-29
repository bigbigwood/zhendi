using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class UserAuthenticationViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Boolean IsSelected { get; set; }
        public String Name { get; set; }
        public Int32 UserAuthenticationID { get; set; }
        public Int32 UserID { get; set; }
        public Int32 DeviceID { get; set; }
        public Int32 DeviceUserID { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public String AuthenticationData { get; set; }
        public String Version { get; set; }
        public Boolean IsDuress { get; set; }
        public String Remark { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand<UserAuthenticationViewModel> ModifyCmd { get; private set; }
        public RelayCommand SelectUserAuthenticationCmd { get; private set; }

        public UserAuthenticationViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            ModifyCmd = new RelayCommand<UserAuthenticationViewModel>(Modify);
            SelectUserAuthenticationCmd = new RelayCommand(SelectUserAuthentication);
            Status = GeneralStatus.Enabled;
        }

        private void Save()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(AuthenticationData))
                    AuthenticationData = AuthenticationData.Replace(' ', '-');

                CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                CreateDate = DateTime.Now;

                RaisePropertyChanged(null);
                Close("");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.UserAuthenticationView_Close);
        }

        private void Modify(UserAuthenticationViewModel userAuthenticationViewModel)
        {
            try
            {
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = userAuthenticationViewModel,
                }, Tokens.UserAuthenticationView_Open);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void SelectUserAuthentication()
        {
            try
            {
                if ((int)AuthenticationType <= (int)AuthenticationType.FingerPrint10 &&
                    (int)AuthenticationType >= (int)AuthenticationType.FingerPrint1 &&
                    IsSelected && string.IsNullOrWhiteSpace(AuthenticationData))
                {
                    string message = string.Format("{0}数据为空，不能启用！", Name);
                    Messenger.Default.Send(new NotificationMessage(message), Tokens.UserDeviceAuthView_ShowNotification);
                    IsSelected = false;
                    RaisePropertyChanged("IsSelected");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
