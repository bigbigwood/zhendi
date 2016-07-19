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
        public virtual Int32 DepartmentID { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual String UserCode { get; set; }
        public virtual String Name { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual String Phone { get; set; }
        public virtual String Photo { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }

        public virtual String LastName { get; set; }
        public virtual String FirstName { get; set; }
        public virtual String Nationality { get; set; }
        public virtual String NativePlace { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual Int32? Marriage { get; set; }
        public virtual Int32? PoliticalStatus { get; set; }
        public virtual Int32? Degree { get; set; }
        public virtual String HomeNumber { get; set; }
        public virtual String EnglishName { get; set; }
        public virtual String Company { get; set; }
        public virtual String TechnicalTitle { get; set; }
        public virtual String TechnicalLevel { get; set; }
        public virtual Int32 IDType { get; set; }
        public virtual String IDNumber { get; set; }
        public virtual String SocialNumber { get; set; }
        public virtual String Email { get; set; }
        public virtual String Address { get; set; }
        public virtual String Postcode { get; set; }
        public virtual String Remark { get; set; }


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
                DepartmentID = user.DepartmentID;
                UserType = user.Type;
                UserCode = user.UserCode;
                Name = user.Name;
                Gender = user.Gender;
                Phone = user.Phone;
                Status = user.Status;
                StartDate = user.StartDate;
                EndDate = user.EndDate;

                LastName = user.UserPropertyInfo.LastName;
                FirstName = user.UserPropertyInfo.FirstName;
                Nationality = user.UserPropertyInfo.Nationality;
                NativePlace = user.UserPropertyInfo.NativePlace;
                Birthday = user.UserPropertyInfo.Birthday;
                Marriage = user.UserPropertyInfo.Marriage;
                PoliticalStatus = user.UserPropertyInfo.PoliticalStatus;
                Degree = user.UserPropertyInfo.Degree;
                HomeNumber = user.UserPropertyInfo.HomeNumber;
                EnglishName = user.UserPropertyInfo.EnglishName;
                Company = user.UserPropertyInfo.Company;
                TechnicalTitle = user.UserPropertyInfo.TechnicalTitle;
                TechnicalLevel = user.UserPropertyInfo.TechnicalLevel;
                IDType = user.UserPropertyInfo.IDType;
                IDNumber = user.UserPropertyInfo.IDNumber;
                SocialNumber = user.UserPropertyInfo.SocialNumber;
                Email = user.UserPropertyInfo.Email;
                Address = user.UserPropertyInfo.Address;
                Postcode = user.UserPropertyInfo.Postcode;
                Remark = user.UserPropertyInfo.Remark;
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
