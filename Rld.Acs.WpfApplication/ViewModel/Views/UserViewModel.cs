using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class UserViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public string Title { get; set; }
        public string Avator { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }
        public string UserCode { get; set; }
        public string Phone { get; set; }
        public User CurrentUser { get; set; }

        public UserViewModel(User user)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            CurrentUser = user;

            if (user.UserID != 0)
            {
                //Avator = user.Phone;
                Avator = @"C:\Users\wood\Desktop\aaa.jpg";
                Name = user.Name;
                Gender = user.Gender.ToString();
                Name = user.Name;
                UserCode = user.UserCode;
                Phone = user.Phone;
            }

            Title = (user.UserID != 0) ? "修改人员" : "新增人员";
        }


        private void Save()
        {
            string message = "";
            try
            {
                if (CurrentUser.UserID == 0)
                {
                    //CurrentUser.TimeZoneName = Name;
                    //CurrentUser.Status = GeneralStatus.Enabled;
                    //CurrentUser.CreateUserID = 1;
                    //CurrentUser.CreateDate = DateTime.Now;
                    //CurrentUser.TimeGroupAssociations = GetTimeGroupAssociations();
                    CurrentUser = _userRepo.Insert(CurrentUser);

                    message = "增加人员成功!";
                }
                else
                {
                    //CurrentUser.TimeZoneName = Name;
                    //CurrentUser.Status = GeneralStatus.Enabled;
                    //CurrentUser.UpdateUserID = 1;
                    //CurrentUser.UpdateDate = DateTime.Now;
                    //CurrentUser.TimeGroupAssociations = GetTimeGroupAssociations();
                    _userRepo.Update(CurrentUser);

                    message = "修改人员成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update user fails.", ex);
                message = "保存人员失败";
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseTimeZoneView);
        }   
    }
}
