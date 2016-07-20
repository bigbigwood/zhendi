using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Validator;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class UserViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public List<Department> AuthorizationDepartments { get; set; }
        public List<SysDictionary> NationalityList { get; set; }
        public SysDictionary GenderInfo { get; set; }
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
        public virtual Int32 Nationality { get; set; }
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
            NationalityList = DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.Nationality);

            DepartmentInfo = new Department();

            IsAddMode = userInfo.UserID == 0;
            StartDate = DateTime.Now;
            Birthday = DateTime.Now;
            Avator = @"C:\Users\wood\Desktop\bbb.jpg";
            Title = IsAddMode ? "新增人员" : "修改人员";


            if (!IsAddMode) //Edit mode
            {
                Avator = @"C:\Users\wood\Desktop\aaa.jpg";
                UserType = userInfo.Type;
                UserCode = userInfo.UserCode;
                Name = userInfo.Name;
                Gender = userInfo.Gender;
                Phone = userInfo.Phone;
                Status = userInfo.Status;
                StartDate = userInfo.StartDate;
                EndDate = userInfo.EndDate;
                DepartmentInfo = AuthorizationDepartments.FirstOrDefault(d => d.DepartmentID == userInfo.DepartmentID);

                LastName = userInfo.UserPropertyInfo.FirstName;
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

            GenderInfo = DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.Gender)
                .FirstOrDefault(x => x.ItemID == (int)userInfo.Gender);
        }


        private void Save()
        {
            string message = "";
            try
            {

                ToUser();

                var userValidator = NinjectBinder.GetValidator<UserValidator>();
                var results = userValidator.Validate(CurrentUser);
                if (!results.IsValid)
                {
                    message = string.Join(",", results.Errors);
                    return;
                }

                if (CurrentUser.UserID == 0)
                {
                    CurrentUser = _userRepo.Insert(CurrentUser);
                    message = "增加人员成功!";
                }
                else
                {
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

        private void ToUser()
        {
            //Avator = @"C:\Users\wood\Desktop\aaa.jpg";

            CurrentUser.Photo = Avator;
            CurrentUser.Type = UserType;
            CurrentUser.UserCode = UserCode;
            CurrentUser.Name = Name;
            CurrentUser.Gender =  Gender;
            CurrentUser.Phone = Phone;
            CurrentUser.Status =  Status;
            CurrentUser.StartDate =  StartDate;
            CurrentUser.EndDate =  EndDate;
            CurrentUser.DepartmentID = DepartmentInfo.DepartmentID;

            CurrentUser.UserPropertyInfo.LastName =  LastName;
            CurrentUser.UserPropertyInfo.FirstName =  FirstName;
            CurrentUser.UserPropertyInfo.Nationality =  Nationality;
            CurrentUser.UserPropertyInfo.NativePlace =  NativePlace;
            CurrentUser.UserPropertyInfo.Birthday =  Birthday;
            CurrentUser.UserPropertyInfo.Marriage =  Marriage;
            CurrentUser.UserPropertyInfo.PoliticalStatus = (int)PoliticalStatus;
            CurrentUser.UserPropertyInfo.Degree = (int) Degree;
            CurrentUser.UserPropertyInfo.HomeNumber =  HomeNumber;
            CurrentUser.UserPropertyInfo.EnglishName =  EnglishName;
            CurrentUser.UserPropertyInfo.Company =  Company;
            CurrentUser.UserPropertyInfo.TechnicalTitle =  TechnicalTitle;
            CurrentUser.UserPropertyInfo.TechnicalLevel =  TechnicalLevel;
            CurrentUser.UserPropertyInfo.IDType =  IDType;
            CurrentUser.UserPropertyInfo.IDNumber =  IDNumber;
            CurrentUser.UserPropertyInfo.SocialNumber =  SocialNumber;
            CurrentUser.UserPropertyInfo.Email =  Email;
            CurrentUser.UserPropertyInfo.Address =  Address;
            CurrentUser.UserPropertyInfo.Postcode =  Postcode;
            CurrentUser.UserPropertyInfo.Remark =  Remark;
        }
    }
}
