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
        public RelayCommand UploadFingerPrintPicCmd { get; private set; }
        public RelayCommand<UserAuthenticationViewModel> ModifyCmd { get; private set; }

        public UserAuthenticationViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            UploadFingerPrintPicCmd = new RelayCommand(UploadFingerPrintPic);
            ModifyCmd = new RelayCommand<UserAuthenticationViewModel>(Modify);
            Status = GeneralStatus.Enabled;
        }

        private void Save()
        {
            RaisePropertyChanged(null);
            Close("");
        }
        private void UploadFingerPrintPic()
        {
            AuthenticationData = @"04-20-15-00-E6-D2-B5-60-02-AC-16-00-12-23-B6-D8-45-2C-C0-6A-22-B3-B6-DE-85-5B-C4-02-62-B3-B7-6D-87-59-B8-45-9A-03-B7-E3-44-7A-98-85-6D-4C-76-DD-45-09-00-00-76-0C-B8-01-C8-4A-A4-87-C6-84-B7-04-8A-89-AC-44-39-73-B7-EA-85-A9-98-C8-29-1B-8A-0E-86-0B-00-00-4E-A2-B5-D7-84-5A-98-8A-F5-3A-B7-63-05-49-A8-85-AD-B3-B9-08-44-5C-90-84-65-D2-89-74-09-8B-05-00-B6-BA-B5-D9-C2-4B-C4-05-75-CC-8A-12-85-DB-05-00-ED-D4-B8-12-87-28-BC-44-12-C5-B6-E5-4B-79-88-C2-4A-22-B5-D9-83-BC-94-87-B6-B3-B4-D4-41-3A-C8-4B-E6-24-86-DD-85-0B-00-00-29-D5-85-51-08-0B-00-00-55-CD-89-8E-85-09-00-00-B5-D1-B8-E1-05-1A-A4-83-F2-01-76-D9-02-C8-08-00-E9-01-78-6E-02-09-09-00-12-F2-89-0B-06-4A-09-00-3A-82-B6-DB-02-5A-A0-41-36-1A-B8-E5-47-29-A0-05-92-11-48-DA-02-3C-06-00-45-EB-43-66-01-EB-02-00-8E-0B-44-5F-83-CA-01-E0-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-C5-6D-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-F7-6E";
            AuthenticationData = AuthenticationData.Replace('-', ' ');
            RaisePropertyChanged(null);
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
    }
}
