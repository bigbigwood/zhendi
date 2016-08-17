using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FluentValidation.Results;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using MahApps.Metro.Controls.Dialogs;
using Ninject.Activation.Caching;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Validator;
using ComboBoxItem = Rld.Acs.WpfApplication.Models.ComboBoxItem;
using Department = Rld.Acs.Model.Department;
using DeviceController = Rld.Acs.Model.DeviceController;
using ListBoxItem = Rld.Acs.WpfApplication.Models.ListBoxItem;
using DSProxy = Rld.Acs.WpfApplication.Service.DeviceService;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SyncUserViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand<TreeViewNode> SelectedTreeNodeChangedCmd { get; private set; }
        public RelayCommand MoveToSelectedCmd { get; private set; }
        public RelayCommand RemoveSelectedCmd { get; private set; }
        public RelayCommand SelectedAllSourceUserCmd { get; private set; }
        public RelayCommand SelectedAllTargetUserCmd { get; private set; }


        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<Department> AuthorizationDepartments { get; set; }

        public SyncUserType SyncUserType { get; set; }
        public ObservableCollection<SelectableItem> DeviceDtos { get; set; }
        public List<TreeViewNode> TreeViewSource { get; private set; }
        public ObservableCollection<SelectableItem> DepartmentUserDtos { get; set; }
        public ObservableCollection<SelectableItem> SelectedSyncUserDtos { get; set; }



        public SyncUserViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(ShowUserBySelectedDepartmentNode);
            MoveToSelectedCmd = new RelayCommand(MoveToSelected);
            RemoveSelectedCmd = new RelayCommand(RemoveSelected);
            SelectedAllSourceUserCmd = new RelayCommand(SelectedAllSourceUser);
            SelectedAllTargetUserCmd = new RelayCommand(SelectedAllTargetUser);

            DeviceDtos = new ObservableCollection<SelectableItem>();
            DepartmentUserDtos = new ObservableCollection<SelectableItem>();
            SelectedSyncUserDtos = new ObservableCollection<SelectableItem>();

            AuthorizationDevices = ApplicationManager.GetInstance().AuthorizationDevices;
            AuthorizationDevices.ForEach(d => DeviceDtos.Add(new ListBoxItem { ID = d.DeviceID, DisplayName = d.Code }));

            AuthorizationDepartments = AuthorizationDepartments = ApplicationManager.GetInstance().AuthorizationDepartments;
            TreeViewSource = BuildTreeViewSource();
        }


        private void Save()
        {
            string question = "确定同步数据吗？";
            Messenger.Default.Send(new NotificationMessageAction(this, question, SyncData), Tokens.SyncUserView_ShowQuestion);
        }

        private void SyncData()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                string message = "";

                var controller = await DialogCoordinator.Instance.ShowProgressAsync(this, "同步数据", "同步数据中，请稍等");
                controller.SetIndeterminate();

                await Task.Run(() =>
                {
                    try
                    {
                        var devices = DeviceDtos.FindAll(d => d.IsSelected).Select(dd => new DSProxy.DeviceController() { DeviceID = dd.ID });
                        var users = SelectedSyncUserDtos.Select(u => new DSProxy.User() { UserID = u.ID });

                        DSProxy.ResultTypes resultTypes;
                        bool resultTypeSpecified;
                        string[] messages;

                        if (SyncUserType == SyncUserType.SyncDeviceToUser)
                        {
                            new DSProxy.DeviceService().SyncDBUsers(devices.ToArray(), users.ToArray(), out resultTypes,
                                out resultTypeSpecified, out messages);
                        }
                        else
                        {
                            new DSProxy.DeviceService().SyncDeviceUsers(devices.ToArray(), users.ToArray(), out resultTypes,
                                out resultTypeSpecified, out messages);
                        }
                        message = "同步数据成功！";
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        message = "同步数据失败！";
                    }
                });

                await controller.CloseAsync();
                Messenger.Default.Send(new NotificationMessage(message), Tokens.SyncUserView_ShowNotification);
            });
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.SyncUserView_ShowNotification);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseSyncUserView);
        }

        private List<TreeViewNode> BuildTreeViewSource()
        {
            var treeViewRoots = new List<TreeViewNode>();

            var rootDepartments = AuthorizationDepartments.FindAll(d => d.Parent == null);
            foreach (var rootDepartment in rootDepartments)
            {
                var rootNode = BuildTreeNode(AuthorizationDepartments, rootDepartment);
                treeViewRoots.Add(rootNode);
            }

            return treeViewRoots;
        }

        private TreeViewNode BuildTreeNode(List<Department> departments, Department rootDepartment)
        {
            var currentNode = new TreeViewNode() { ID = rootDepartment.DepartmentID, Name = rootDepartment.Name };
            var children = departments.FindAll(d => (d.Parent != null && d.Parent.DepartmentID == rootDepartment.DepartmentID));
            foreach (var subDept in children)
            {
                var node = BuildTreeNode(departments, subDept);
                currentNode.Children.Add(node);
            }

            return currentNode;
        }


        private void ShowUserBySelectedDepartmentNode(TreeViewNode selectedNode)
        {
            try
            {
                if (selectedNode.ID == -1)
                    return;

                DepartmentUserDtos = new ObservableCollection<SelectableItem>();
                var users = _userRepo.GetDepartmentSummaryUsers(selectedNode.ID);
                if (users != null && users.Any())
                {
                    users.ForEach(user => DepartmentUserDtos.Add(new ComboBoxItem() { ID = user.UserID, DisplayName = user.Name }));
                }

                RaisePropertyChanged(null);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void MoveToSelected()
        {
            var selectedUsers = DepartmentUserDtos.FindAll(u => u.IsSelected);
            if (selectedUsers == null || !selectedUsers.Any()) return;

            selectedUsers.ForEach(u =>
            {
                if (SelectedSyncUserDtos.All(sy => sy.ID != u.ID))
                {
                    SelectedSyncUserDtos.Add(new ComboBoxItem() { ID = u.ID, DisplayName = u.DisplayName });
                }
            });

            RaisePropertyChanged(null);
        }

        private void RemoveSelected()
        {
            var selectedUsers = SelectedSyncUserDtos.FindAll(u => u.IsSelected);
            if (selectedUsers == null || !selectedUsers.Any()) return;

            selectedUsers.ForEach(u => SelectedSyncUserDtos.Remove(u));
            RaisePropertyChanged(null);
        }

        private void SelectedAllSourceUser()
        {
            var temp = DepartmentUserDtos;
            DepartmentUserDtos = new ObservableCollection<SelectableItem>();
            temp.ForEach(t => DepartmentUserDtos.Add(new ComboBoxItem() { ID = t.ID, DisplayName = t.DisplayName, IsSelected = true }));
            RaisePropertyChanged(null);
        }

        private void SelectedAllTargetUser()
        {
            //SelectedSyncUserDtos.ForEach(u => u.IsSelected = true);
            //RaisePropertyChanged(null);

            var temp = SelectedSyncUserDtos;
            SelectedSyncUserDtos = new ObservableCollection<SelectableItem>();
            temp.ForEach(t => SelectedSyncUserDtos.Add(new ComboBoxItem() { ID = t.ID, DisplayName = t.DisplayName, IsSelected = true }));
            RaisePropertyChanged(null);
        }
    }
}
