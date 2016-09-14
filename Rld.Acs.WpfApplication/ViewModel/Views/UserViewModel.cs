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

        public Boolean IsAddMode { get; set; }
        public string Title { get; set; }
        public string Avator { get; set; }
        public Int32 UserID { get; set; }
        public UserType UserType { get; set; }
        public String UserCode { get; set; }
        public String Name { get; set; }
        public GenderType Gender { get; set; }
        public String Phone { get; set; }
        public String Photo { get; set; }
        public Boolean Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public String LastName { get; set; }
        public String FirstName { get; set; }
        public Int32 Nationality { get; set; }
        public String NativePlace { get; set; }
        public DateTime Birthday { get; set; }
        public Marriage Marriage { get; set; }
        public PoliticalStatus PoliticalStatus { get; set; }
        public DegreeStatus Degree { get; set; }
        public String HomeNumber { get; set; }
        public String EnglishName { get; set; }
        public String Company { get; set; }
        public String TechnicalTitle { get; set; }
        public String TechnicalLevel { get; set; }
        public Int32 IDType { get; set; }
        public String IDNumber { get; set; }
        public String SocialNumber { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
        public String Postcode { get; set; }
        public String Remark { get; set; }
        public String CurrentDepartmentName { get; set; }

        public SysDictionary GenderInfo
        {
            get{ return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.Gender).FirstOrDefault(x => x.ItemID == (int)Gender); }
        }
        public List<Department> AuthorizationDepartments
        {
            get { return ApplicationManager.GetInstance().AuthorizationDepartments.FindAll(x => x.DepartmentID != -1); }
        }
        public List<SysDictionary> NationalityList
        {
            get { return DictionaryManager.GetInstance().GetDictionaryItemsByTypeId((int)DictionaryType.Nationality); }
        }
        public ObservableCollection<ListBoxItem> DeviceRoleListBoxSource { get; set; }

        public ViewModelAttachment<User> ViewModelAttachment { get; set; }
        public virtual Department DepartmentInfo { get; set; }
       

        public User CurrentUser { get; set; }

        public UserViewModel(User userInfo)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            UploadImageCmd = new RelayCommand<string>(UploadImage);
            DepartmentChangedCmd = new RelayCommand(ProcessDepartmentChangedCmd);

            CurrentUser = userInfo;
            DepartmentInfo = new Department();
            UserID = userInfo.UserID;
            IsAddMode = userInfo.UserID == 0;
            StartDate = DateTime.Now;
            Birthday = DateTime.Now;
            UserType = UserType.Employee;
            ViewModelAttachment = new ViewModelAttachment<User>();
            Avator = _userAvatorService.GetAvator(_userAvatorService.DefaultAvatorFileName);
            Title = IsAddMode ? "新增人员" : "修改人员";

            if (!IsAddMode) //Edit mode
            {
                UserType = userInfo.Type;
                UserCode = userInfo.UserCode;
                Name = userInfo.Name;
                Status = userInfo.Status == GeneralStatus.Enabled;
                StartDate = userInfo.StartDate;
                EndDate = userInfo.EndDate;
                Phone = userInfo.Phone;
                Avator = _userAvatorService.GetAvator(userInfo.Photo);
                DepartmentInfo = AuthorizationDepartments.FirstOrDefault(d => d.DepartmentID == userInfo.DepartmentID);
                CurrentDepartmentName = DepartmentInfo.Name;
                Gender = userInfo.Gender;
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

                if (IsAddMode)
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

            ViewModelAttachment.CoreModel = CurrentUser;
            ViewModelAttachment.LastOperationSuccess = true;
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
                string cacheFilePath = string.Format(@"{0}\{1}", ApplicationEnvironment.LocalImageCachePath, uniqueFileName);
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
            CurrentUser.Gender = Gender;
            CurrentUser.Phone = Phone;
            CurrentUser.Status = Status ? GeneralStatus.Enabled : GeneralStatus.Disabled ;
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
