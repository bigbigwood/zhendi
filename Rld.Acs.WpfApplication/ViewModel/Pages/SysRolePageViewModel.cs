﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.ViewModel.Views;
using GalaSoft.MvvmLight.Threading;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class SysRolePageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysRoleRepository _sysRoleRepo = NinjectBinder.GetRepository<ISysRoleRepository>();
        private ISysOperatorRoleRepository _sysOperatorRoleRepo = NinjectBinder.GetRepository<ISysOperatorRoleRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public ObservableCollection<SysRoleViewModel> SysRoleViewModels { get; set; }
        public SysRoleViewModel SelectedSysRoleViewModel { get; set; }

        public SysRolePageViewModel()
        {
            AddCmd = new AuthCommand(Add);
            ModifyCmd = new AuthCommand(Modify);
            DeleteCmd = new AuthCommand(ShowDeletionQuestion);

            var vms = SysPermissionProvider.GetInstance().AllSysRoles.Select(Mapper.Map<SysRoleViewModel>);
            SysRoleViewModels = new ObservableCollection<SysRoleViewModel>(vms);
        }

        private void Add()
        {
            try
            {
                var viewModel = Mapper.Map<SysRoleViewModel>(new SysRole());
                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.SysRoleView_Open);
                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var coreModel = _sysRoleRepo.GetByKey(viewModel.RoleID);
                    viewModel = Mapper.Map<SysRoleViewModel>(coreModel);
                    SysRoleViewModels.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Modify()
        {
            try
            {
                if (SelectedSysRoleViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择有效数据!"), Tokens.SysRolePage_ShowNotification);
                    return;
                }

                var coreModel = Mapper.Map<SysRole>(SelectedSysRoleViewModel);
                var viewModel = Mapper.Map<SysRoleViewModel>(coreModel);
                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.SysRoleView_Open);
                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    viewModel = Mapper.Map<SysRoleViewModel>(viewModel.ViewModelAttachment.CoreModel);
                    var index = SysRoleViewModels.IndexOf(SelectedSysRoleViewModel);
                    SysRoleViewModels[index] = viewModel;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ShowDeletionQuestion()
        {
            try
            {
                if (SelectedSysRoleViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择有效数据!"), Tokens.SysRolePage_ShowNotification);
                    return;
                }

                var operatorRoles = _sysOperatorRoleRepo.Query(new Hashtable() { { "RoleID", SelectedSysRoleViewModel.RoleID } });
                if (operatorRoles.Any())
                {
                    Messenger.Default.Send(new NotificationMessage("该系统角色已经被系统用户使用，不能删除!"), Tokens.SysRolePage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除:{0}吗？", SelectedSysRoleViewModel.RoleName);
                Messenger.Default.Send(new NotificationMessageAction(this, question, Delete), Tokens.SysRolePage_ShowQuestion);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Delete()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _sysRoleRepo.Delete(SelectedSysRoleViewModel.RoleID);
                    message = "删除成功!";

                    SysRoleViewModels.Remove(SelectedSysRoleViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.SysRolePage_ShowNotification);
            });
        }
    }
}
