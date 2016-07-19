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
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class UserViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public List<Department> AuthorizationDepartments { get; set; }
        public Boolean IsAddMode { get; set; }
        public string Title { get; set; }
        public string Avator { get; set; }
        public virtual Department DepartmentInfo { get; set; }
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
        public virtual Marriage Marriage { get; set; }
        public virtual PoliticalStatus PoliticalStatus { get; set; }
        public virtual DegreeStatus Degree { get; set; }
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

        public UserViewModel(User userInfo)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            CurrentUser = userInfo;
            AuthorizationDepartments = ApplicationManager.GetInstance().AuthorizationDepartments;

            IsAddMode = userInfo.UserID == 0;
            StartDate = DateTime.Now;
            Birthday = DateTime.Now;
            Title = IsAddMode ? "新增人员" : "修改人员";


            if (!IsAddMode) //Edit mode
            {
                //Avator = user.Phone;
                Avator = @"C:\Users\wood\Desktop\aaa.jpg";
                //DepartmentID = user.DepartmentID;
                UserType = userInfo.Type;
                UserCode = userInfo.UserCode;
                Name = userInfo.Name;
                Gender = userInfo.Gender;
                Phone = userInfo.Phone;
                Status = userInfo.Status;
                StartDate = userInfo.StartDate;
                EndDate = userInfo.EndDate;
                DepartmentInfo = AuthorizationDepartments.FirstOrDefault(d => d.DepartmentID == userInfo.DepartmentID);

                LastName = userInfo.UserPropertyInfo.LastName;
                FirstName = userInfo.UserPropertyInfo.FirstName;
                Nationality = userInfo.UserPropertyInfo.Nationality;
                NativePlace = userInfo.UserPropertyInfo.NativePlace;
                Birthday = userInfo.UserPropertyInfo.Birthday;
                Marriage = userInfo.UserPropertyInfo.Marriage;
                PoliticalStatus = (userInfo.UserPropertyInfo.PoliticalStatus != null) 
                    ? (PoliticalStatus)userInfo.UserPropertyInfo.PoliticalStatus : PoliticalStatus.Unknown;
                Degree = (userInfo.UserPropertyInfo.Degree != null)
                    ? (DegreeStatus)userInfo.UserPropertyInfo.Degree : DegreeStatus.Unknown;
                HomeNumber = userInfo.UserPropertyInfo.HomeNumber;
                EnglishName = userInfo.UserPropertyInfo.EnglishName;
                Company = userInfo.UserPropertyInfo.Company;
                TechnicalTitle = userInfo.UserPropertyInfo.TechnicalTitle;
                TechnicalLevel = userInfo.UserPropertyInfo.TechnicalLevel;
                IDType = userInfo.UserPropertyInfo.IDType;
                IDNumber = userInfo.UserPropertyInfo.IDNumber;
                SocialNumber = userInfo.UserPropertyInfo.SocialNumber;
                Email = userInfo.UserPropertyInfo.Email;
                Address = userInfo.UserPropertyInfo.Address;
                Postcode = userInfo.UserPropertyInfo.Postcode;
                Remark = userInfo.UserPropertyInfo.Remark;
            }
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
