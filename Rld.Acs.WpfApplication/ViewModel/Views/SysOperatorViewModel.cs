using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Validator;


namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SysOperatorViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysOperatorRepository _sysOperatorRepo = NinjectBinder.GetRepository<ISysOperatorRepository>();

        public Int32 OperatorID { get; set; }
        public Int32? UserID { get; set; }
        public String LoginName { get; set; }
        public String Password { get; set; }
        public String Salt { get; set; }
        public Int32 LanguageID { get; set; }
        public String Photo { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public String Title { get; set; }

        private Boolean _newPasswordEnabled;
        public Boolean NewPasswordEnabled
        {
            get { return _newPasswordEnabled; }
            set
            {
                if (_newPasswordEnabled != value)
                {
                    _newPasswordEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }
        public String NewPassword1 { get; set; }
        public String NewPassword2 { get; set; }
        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public ObservableCollection<ComboBoxItem> SysOperatorRoleItems { get; set; }
        public SysOperator NewCoreModel { get; set; }

        public SysOperatorViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            SysOperatorRoleItems = new ObservableCollection<ComboBoxItem>();
            SysPermissionProvider.GetInstance().AllSysRoles.ForEach(r =>
                SysOperatorRoleItems.Add(new ComboBoxItem() {ID =  r.RoleID, DisplayName = r.RoleName}));
        }

        private void Save()
        {
            string message = "";
            try
            {
                if (NewPasswordEnabled)
                {
                    if (string.IsNullOrWhiteSpace(NewPassword1))
                        message += "\n密码不能为空";
                    else if (string.IsNullOrWhiteSpace(NewPassword2))
                        message += "\n确认密码不能为空";
                    else if (NewPassword1 != NewPassword2)
                    {
                        message += "\n两次输入密码不一致";
                    }

                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        message = message.Trim('\n');
                        SendMessage(message);
                        return;
                    }

                    Password = PasswordService.ExcryptPassword(NewPassword1);
                }

                var coreModel = Mapper.Map<SysOperator>(this);
                var validator = NinjectBinder.GetValidator<SysOperatorValidator>();
                var results = validator.Validate(coreModel);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                if (OperatorID == 0)
                {
                    coreModel.CreateDate = DateTime.Now;
                    coreModel.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    coreModel = _sysOperatorRepo.Insert(coreModel);
                    NewCoreModel = coreModel;
                    message = "增加成功!";
                }
                else
                {
                    coreModel.UpdateDate = DateTime.Now;
                    coreModel.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    _sysOperatorRepo.Update(coreModel);
                    message = "修改成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update sys role fails.", ex);
                message = "保存失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.SysOperatorView_Close);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.SysOperatorView_ShowNotification);
        }

        public List<SysOperatorRole> GetRolesFromUI(SysOperator coreModel)
        {
            var result = new List<SysOperatorRole>();
            SysOperatorRoleItems.FindAll(x => x.IsSelected).ForEach(x =>
            {
                var role =coreModel.SysOperatorRoles.FirstOrDefault(r => r.RoleID == x.ID);
                if (role == null)
                {
                    role = new SysOperatorRole() {OperatorID = OperatorID, RoleID = x.ID};
                }
                result.Add(role);
            });

            return result;
        }

        public void BindUI(SysOperator coreModel)
        {
            if (OperatorID == 0)
            {
                NewPasswordEnabled = true;
                LanguageID = 2052;
                Status = GeneralStatus.Enabled;
            }
            else
            {
                coreModel.SysOperatorRoles.ForEach(x =>
                {
                    var item = SysOperatorRoleItems.FirstOrDefault(i => i.ID == x.RoleID);
                    if (item != null) item.IsSelected = true;
                });
            }
        }
    }
}
