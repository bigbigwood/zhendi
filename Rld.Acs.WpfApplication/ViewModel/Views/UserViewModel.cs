using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Ninject.Activation.Caching;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Validator;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class UserViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();
        private UserAvatorService _userAvatorService = new UserAvatorService();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand<string> UploadImageCmd { get; private set; }
        public RelayCommand DepartmentChangedCmd { get; private set; }


        public List<Department> AuthorizationDepartments { get; set; }
        public List<SysDictionary> NationalityList { get; set; }
        public ObservableCollection<ListBoxItem> DeviceRoleListBoxSource { get; set; }


        public virtual Department DepartmentInfo { get; set; }

        public Boolean IsAddMode { get; set; }
        public string Title { get; set; }
        public string Avator { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual String UserCode { get; set; }
        public virtual String Name { get; set; }
        public SysDictionary GenderInfo { get; set; }
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
        public virtual String CurrentDepartmentName { get; set; }


        public User CurrentUser { get; set; }

        public UserViewModel(User userInfo)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            UploadImageCmd = new RelayCommand<string>(UploadImage);
            DepartmentChangedCmd = new RelayCommand(ProcessDepartmentChangedCmd);

            CurrentUser = userInfo;
            AuthorizationDepartments = ApplicationManager.GetInstance().AuthorizationDepartments;
            NationalityList = DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.Nationality);

            DepartmentInfo = new Department();
            GenderInfo =
                DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int) DictionaryType.Gender).FirstOrDefault();

            IsAddMode = userInfo.UserID == 0;
            StartDate = DateTime.Now;
            Birthday = DateTime.Now;
            Avator = _userAvatorService.GetAvator(_userAvatorService.DefaultAvatorFileName);
            Title = IsAddMode ? "新增人员" : "修改人员";

            if (!IsAddMode) //Edit mode
            {
                UserType = userInfo.Type;
                UserCode = userInfo.UserCode;
                Name = userInfo.Name;
                Status = userInfo.Status;
                StartDate = userInfo.StartDate;
                EndDate = userInfo.EndDate;
                Avator = _userAvatorService.GetAvator(userInfo.Photo);
                GenderInfo = DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.Gender)
                    .FirstOrDefault(x => x.ItemID == (int)userInfo.Gender);
                DepartmentInfo = AuthorizationDepartments.FirstOrDefault(d => d.DepartmentID == userInfo.DepartmentID);
                CurrentDepartmentName = DepartmentInfo.Name;

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

            DeviceRoleListBoxSource = GetDeviceRoleListBoxSource();
        }

        private void Save()
        {
            string message = "";
            try
            {
                ToUser();

                var validator = NinjectBinder.GetValidator<UserValidator>();
                var results = validator.Validate(CurrentUser);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
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
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.UserView_ShowNotification);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseUserView);
        }

        private void UploadImage(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new Exception("file does not exist.");

                string extension = new FileInfo(filePath).Extension;

                string uniqueFileName = string.Format(@"{0}_{1}{2}", Guid.NewGuid(), DateTime.Now.ToString("yyyyMMddhhmmss"), extension);
                string cacheFilePath = string.Format(@"{0}\{1}", ApplicationManager.GetInstance().LocalImageCachePath, uniqueFileName);
                File.Copy(filePath, cacheFilePath);

                _userAvatorService.UploadAvatorToServer(uniqueFileName);
                Avator = cacheFilePath;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        private void ProcessDepartmentChangedCmd()
        {
            if (IsAddMode)
            {
                //Apply default role of department
                CurrentUser.UserDeviceRoles = new []{ new UserDeviceRole{ DeviceRoleID = DepartmentInfo.DeviceRoleID }}.ToList();
            }
            else
            {
                //Change default role of department
                var previousDefaultItem = DeviceRoleListBoxSource.First(x => x.IsDefault);
                var departmentDefaultRole = CurrentUser.UserDeviceRoles.First(x => x.DeviceRoleID == previousDefaultItem.ID);
                departmentDefaultRole.DeviceRoleID = DepartmentInfo.DeviceRoleID;
            }

            DeviceRoleListBoxSource = GetDeviceRoleListBoxSource();
            RaisePropertyChanged(null);
        }


        private ObservableCollection<ListBoxItem> GetDeviceRoleListBoxSource()
        {
            var items = new ObservableCollection<ListBoxItem>();
            var AuthorizationDeviceRoles = ApplicationManager.GetInstance().AuthorizationDeviceRoles;
            AuthorizationDeviceRoles.ForEach(role => items.Add(new ListBoxItem()
            {
                ID = role.DeviceRoleID, 
                DisplayName = role.RoleName, 
                IsSelected = CurrentUser.UserDeviceRoles.Select(x => x.DeviceRoleID).Contains(role.DeviceRoleID), 
                IsDefault = (DepartmentInfo.DeviceRoleID == role.DeviceRoleID),
                IsEnabled = (DepartmentInfo.DeviceRoleID != role.DeviceRoleID), // default is not able to changed
            }));

            return items;
        }

        private List<UserDeviceRole> GetUserDeviceFromUI()
        {
            List<UserDeviceRole> result = new List<UserDeviceRole>();
            var AuthorizationDeviceRoles = ApplicationManager.GetInstance().AuthorizationDeviceRoles;
            var selected = DeviceRoleListBoxSource.FindAll(x => x.IsSelected);
            foreach (var listBoxItem in selected)
            {
                var userDeviceRole = CurrentUser.UserDeviceRoles.FirstOrDefault(x => x.DeviceRoleID == listBoxItem.ID);
                if (userDeviceRole == null)
                {
                    userDeviceRole = new UserDeviceRole() {DeviceRoleID = listBoxItem.ID, UserID = CurrentUser.UserID};
                }

                result.Add(userDeviceRole);
            }
            return result;
        }

        private void ToUser()
        {
            //Avator = @"C:\Users\wood\Desktop\aaa.jpg";
            CurrentUser.Photo = new FileInfo(Avator).Name;
            CurrentUser.Type = UserType;
            CurrentUser.UserCode = UserCode;
            CurrentUser.Name = Name;
            CurrentUser.Gender = (GenderType)GenderInfo.ItemID;
            CurrentUser.Phone = Phone;
            CurrentUser.Status = Status;
            CurrentUser.StartDate = StartDate;
            CurrentUser.EndDate = EndDate;
            CurrentUser.DepartmentID = DepartmentInfo.DepartmentID;

            CurrentUser.UserPropertyInfo.LastName = LastName;
            CurrentUser.UserPropertyInfo.FirstName = FirstName;
            CurrentUser.UserPropertyInfo.Nationality = Nationality;
            CurrentUser.UserPropertyInfo.NativePlace = NativePlace;
            CurrentUser.UserPropertyInfo.Birthday = Birthday;
            CurrentUser.UserPropertyInfo.Marriage = Marriage;
            CurrentUser.UserPropertyInfo.PoliticalStatus = (int)PoliticalStatus;
            CurrentUser.UserPropertyInfo.Degree = (int)Degree;
            CurrentUser.UserPropertyInfo.HomeNumber = HomeNumber;
            CurrentUser.UserPropertyInfo.EnglishName = EnglishName;
            CurrentUser.UserPropertyInfo.Company = Company;
            CurrentUser.UserPropertyInfo.TechnicalTitle = TechnicalTitle;
            CurrentUser.UserPropertyInfo.TechnicalLevel = TechnicalLevel;
            CurrentUser.UserPropertyInfo.IDType = IDType;
            CurrentUser.UserPropertyInfo.IDNumber = IDNumber;
            CurrentUser.UserPropertyInfo.SocialNumber = SocialNumber;
            CurrentUser.UserPropertyInfo.Email = Email;
            CurrentUser.UserPropertyInfo.Address = Address;
            CurrentUser.UserPropertyInfo.Postcode = Postcode;
            CurrentUser.UserPropertyInfo.Remark = Remark;

            CurrentUser.UserDeviceRoles = GetUserDeviceFromUI();
        }
    }
}
