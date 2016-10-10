using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using AutoMapper;
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
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Validator;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class UserAuthMngtViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();
        public ObservableCollection<UserDeviceAuthViewModel> UserDeviceAuthViewModels { get; set; }
        public User CurrentUser { get; set; }
        public ViewModelAttachment<User> ViewModelAttachment { get; set; }
        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public UserAuthMngtViewModel(User currentUser)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            CurrentUser = currentUser;
            ViewModelAttachment = new ViewModelAttachment<User>();
            UserDeviceAuthViewModels = new ObservableCollection<UserDeviceAuthViewModel>(CreateUserDeviceAuthViewModels());
        }

        private List<UserDeviceAuthViewModel> CreateUserDeviceAuthViewModels()
        {
            var result = new List<UserDeviceAuthViewModel>();
            var authDeviceIds = CurrentUser.GetUserRoleAuthorizedDeviceIds(ApplicationManager.GetInstance().AuthorizationDeviceRoles);
            foreach (var authDeviceId in authDeviceIds)
            {
                var userDeviceAuthViewModel = new UserDeviceAuthViewModel(authDeviceId, CurrentUser.UserID, CurrentUser.UserCode.ToInt32());
                userDeviceAuthViewModel.DeviceName = ApplicationManager.GetInstance().AuthorizationDevices.FirstOrDefault(x => x.DeviceID == authDeviceId).Name;

                var auths = CurrentUser.UserAuthentications.FindAll(x => x.DeviceID == authDeviceId);
                auths.ForEach(x =>
                {
                    if (x.AuthenticationType == AuthenticationType.Password)
                    {
                        userDeviceAuthViewModel.PasswordCredential = Mapper.Map<UserAuthenticationViewModel>(x);
                        userDeviceAuthViewModel.PasswordCredential.IsSelected = true;
                    }
                    else if (x.AuthenticationType == AuthenticationType.IcCard)
                    {
                        userDeviceAuthViewModel.IcCardCredential = Mapper.Map<UserAuthenticationViewModel>(x);
                        userDeviceAuthViewModel.IcCardCredential.IsSelected = true;
                    }
                    else
                    {
                        var fp = userDeviceAuthViewModel.FingerPrintCredentials.FirstOrDefault(e => e.AuthenticationType == x.AuthenticationType);
                        if (fp != null)
                        {
                            var fpname = fp.Name;
                            var index = userDeviceAuthViewModel.FingerPrintCredentials.IndexOf(fp);
                            userDeviceAuthViewModel.FingerPrintCredentials[index] = Mapper.Map<UserAuthenticationViewModel>(x);
                            userDeviceAuthViewModel.FingerPrintCredentials[index].IsSelected = true;
                            userDeviceAuthViewModel.FingerPrintCredentials[index].Name = fpname;
                        }
                    }

                });

                result.Add(userDeviceAuthViewModel);
            }
            return result;
        }

        private List<UserAuthentication> GetAuthentications()
        {
            var result = new List<UserAuthentication>();
            UserDeviceAuthViewModels.ForEach(x =>
            {
                if(x.PasswordCredential.IsSelected)
                    result.Add(Mapper.Map<UserAuthentication>(x.PasswordCredential));

                if (x.IcCardCredential.IsSelected)
                    result.Add(Mapper.Map<UserAuthentication>(x.IcCardCredential));

                result.AddRange(x.FingerPrintCredentials.FindAll(xx => xx.IsSelected).Select(Mapper.Map<UserAuthentication>));
            });

            return result;
        }


        private void Save()
        {
            string message = "";
            try
            {
                string errorMessage = "";
                var passwordCredentials = UserDeviceAuthViewModels.Select(x => x.PasswordCredential);
                var validator = NinjectBinder.GetValidator<PasswordAuthenticationViewModelValidator>();
                passwordCredentials.ForEach(x =>
                {
                    var results = validator.Validate(x);
                    if (!results.IsValid)
                        errorMessage = string.Join("\n", results.Errors);
                });
                var icCardCredentials = UserDeviceAuthViewModels.Select(x => x.IcCardCredential);
                var icCardValidator = NinjectBinder.GetValidator<IcCardAuthenticationViewModelValidator>();
                icCardCredentials.ForEach(x =>
                {
                    var results = icCardValidator.Validate(x);
                    if (!results.IsValid)
                        errorMessage = string.Join("\n", results.Errors);
                });
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    SendMessage(errorMessage);
                    return;
                }

                CurrentUser.UserAuthentications = GetAuthentications();
                CurrentUser.UserAuthentications.FindAll(x => x.UserAuthenticationID == 0).ForEach(xx =>
                {
                    xx.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    xx.CreateDate = DateTime.Now;
                });
                CurrentUser.UserAuthentications.FindAll(x => x.UserAuthenticationID != 0).ForEach(xx =>
                {
                    xx.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    xx.UpdateDate = DateTime.Now;
                });

                _userRepo.Update(CurrentUser);
                message = "更新凭证成功!";
            }
            catch (Exception ex)
            {
                Log.Error("Update user fails.", ex);
                message = "更新凭证失败";
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
            Messenger.Default.Send(new NotificationMessage(message), Tokens.UserDeviceAuthView_ShowNotification);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.UserDeviceAuthView_Close);
        }
    }

    public class UserDeviceAuthViewModel : ViewModelBase
    {
        public string DeviceName { get; set; }
        public ObservableCollection<UserAuthenticationViewModel> FingerPrintCredentials { get; set; }
        public UserAuthenticationViewModel PasswordCredential { get; set; }
        public UserAuthenticationViewModel IcCardCredential { get; set; }

        public UserDeviceAuthViewModel(int DeviceId, int UserId, int DeviceUserId)
        {
            var fp1 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint1, Name = "指纹一"};
            var fp2 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint2, Name = "指纹二" };
            var fp3 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint3, Name = "指纹三" };
            var fp4 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint4, Name = "指纹四" };
            var fp5 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint5, Name = "指纹五" };
            var fp6 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint6, Name = "指纹六" };
            var fp7 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint7, Name = "指纹七" };
            var fp8 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint8, Name = "指纹八" };
            var fp9 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint9, Name = "指纹九" };
            var fp10 = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.FingerPrint10, Name = "指纹十" };

            var auths = new List<UserAuthenticationViewModel> {fp1,fp2,fp3,fp4,fp5,fp6,fp7,fp8,fp9,fp10};
            FingerPrintCredentials = new ObservableCollection<UserAuthenticationViewModel>(auths);

            PasswordCredential = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.Password };
            IcCardCredential = new UserAuthenticationViewModel { UserID = UserId, DeviceID = DeviceId, DeviceUserID = DeviceUserId, AuthenticationType = AuthenticationType.IcCard };
        }
    }
}
