﻿using System;
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
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class MoveUserViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public ViewModelAttachment<User> ViewModelAttachment { get; set; }
        public User CurrentUser { get; set; }
        public Department CurrentDepartment { get; set; }
        public Department NewDepartment { get; set; }
        public List<Department> AuthorizationDepartments
        {
            get { return ApplicationManager.GetInstance().AuthorizationDepartments.FindAll(x => x.DepartmentID != -1); }
        }
        public MoveUserViewModel(User userInfo)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            ViewModelAttachment = new ViewModelAttachment<User>();
            CurrentUser = userInfo;
            CurrentDepartment = AuthorizationDepartments.First(x => x.DepartmentID == userInfo.DepartmentID);
        }

        private void Save()
        {
            var message = "";
            try
            {
                if (NewDepartment.DepartmentCode == CurrentDepartment.DepartmentCode)
                {
                    Messenger.Default.Send(new NotificationMessage(this, "不能移动到原部门"), Tokens.MoveUserView_ShowNotification);
                    return;
                }

                CurrentUser.DepartmentID = NewDepartment.DepartmentID;
                UpdateDeviceRoleFromNewDepartment(CurrentUser, CurrentDepartment, NewDepartment);

                _userRepo.Update(CurrentUser);
                message = "移动人员成功!";
            }
            catch (Exception ex)
            {
                Log.Error("Move user fails.", ex);
                message = "移动人员失败";
                Messenger.Default.Send(new NotificationMessage(message), Tokens.MoveUserView_ShowNotification);
                return;
            }

            ViewModelAttachment.CoreModel = CurrentUser;
            ViewModelAttachment.LastOperationSuccess = true;
            RaisePropertyChanged(null);
            Close(message);
        }

        private void UpdateDeviceRoleFromNewDepartment(User user, Department currentDepartment, Department newDepartment)
        {
            var currentDepartmentRoleId = currentDepartment.DeviceRoleID;
            var newDepartmentRoleId = newDepartment.DeviceRoleID;

            var currentDepartmentRoles = user.UserDeviceRoles.FindAll(x => x.DeviceRoleID == currentDepartmentRoleId);
            if (currentDepartmentRoles.Any())
            {
                currentDepartmentRoles.ForEach(x => user.UserDeviceRoles.Remove(x));
            }

            user.UserDeviceRoles.Add(new UserDeviceRole() { UserID = user.UserID, DeviceRoleID = newDepartmentRoleId });
        }
 

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.MoveUserView_Close);
        }
    }
}
