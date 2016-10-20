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
using Org.BouncyCastle.Bcpg;
using Rld.Acs.Model;
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
using DSProxy = Rld.Acs.WpfApplication.DeviceProxy;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SyncUserViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand<TreeViewNode> SelectedTreeNodeChangedCmd { get; private set; }
        public RelayCommand QueryDeviceUserCmd { get; private set; }
        public RelayCommand MoveToSelectedCmd { get; private set; }
        public RelayCommand RemoveSelectedCmd { get; private set; }
        public RelayCommand SelectedAllSourceUserCmd { get; private set; }
        public RelayCommand SelectedAllTargetUserCmd { get; private set; }


        public List<DeviceController> AuthorizationDevices
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices; }
        }

        public List<Department> AuthorizationDepartments
        {
            get { return ApplicationManager.GetInstance().AuthorizationDepartments; }
        }
        public DeviceController SelectedDevice { get; set; }
        public string UserCode { get; set; }
        public SyncUserType SyncUserType { get; set; }
        public ObservableCollection<SelectableItem> DeviceDtos { get; set; }

        public List<TreeViewNode> TreeViewSource
        {
            get { return BuildTreeViewSource(); }
        }

        private ObservableCollection<SelectableItem> _departmentUserDtos;
        public ObservableCollection<SelectableItem> DepartmentUserDtos
        {
            get { return _departmentUserDtos; }
            set
            {
                if (_departmentUserDtos != value)
                {
                    _departmentUserDtos = value;
                    RaisePropertyChanged();
                }
            }
        }
        private ObservableCollection<SelectableItem> _deviceUserDtos;
        public ObservableCollection<SelectableItem> DeviceUserDtos
        {
            get { return _deviceUserDtos; }
            set
            {
                if (_deviceUserDtos != value)
                {
                    _deviceUserDtos = value;
                    RaisePropertyChanged();
                }
            }
        }

        public SelectableItem SelectedDeviceUser { get; set; }

        private ObservableCollection<SelectableItem> _selectedSyncUserDtos;

        public ObservableCollection<SelectableItem> SelectedSyncUserDtos
        {
            get { return _selectedSyncUserDtos; }
            set
            {
                if (_selectedSyncUserDtos != value)
                {
                    _selectedSyncUserDtos = value;
                    RaisePropertyChanged();
                }
            }
        } 



        public SyncUserViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(ShowUserBySelectedDepartmentNode);
            MoveToSelectedCmd = new RelayCommand(MoveToSelected);
            RemoveSelectedCmd = new RelayCommand(RemoveSelected);
            SelectedAllSourceUserCmd = new RelayCommand(SelectAllDepartmentUsers);
            SelectedAllTargetUserCmd = new RelayCommand(RemoveAllSelectedUsers);
            QueryDeviceUserCmd = new RelayCommand(QueryDeviceUser);

            DepartmentUserDtos = new ObservableCollection<SelectableItem>();
            SelectedSyncUserDtos = new ObservableCollection<SelectableItem>();
            DeviceUserDtos = new ObservableCollection<SelectableItem>();

            var dtos = AuthorizationDevices.Select(x => new ListBoxItem {ID = x.DeviceID, DisplayName = x.Name});
            DeviceDtos = new ObservableCollection<SelectableItem>(dtos);
        }

        private void Save()
        {
            if (SyncUserType == SyncUserType.SyncUserToDevice)
            {
                var validator = NinjectBinder.GetValidator<SyncUserViewModelValidator>();
                var results = validator.Validate(this);
                if (!results.IsValid)
                {
                    var message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }
            }

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
 
                        DSProxy.ResultTypes resultTypes;
                        string[] messages;

                        if (SyncUserType == SyncUserType.SyncDeviceToUser)
                        {
                            var devices = new[] { SelectedDevice };
                            var users = SelectedSyncUserDtos.Select(u =>
                            {
                                var userId = string.IsNullOrWhiteSpace(u.Description) ? 0 : u.Description.ToInt32();
                                var userCode = u.ID.ToString();
                                return new User() {UserID = userId, UserCode = userCode, Name = u.DisplayName};
                            });

                            resultTypes = new DSProxy.DeviceServiceClient().SyncSystemUsers(devices.ToArray(), users.ToArray(), out messages);
                        }
                        else
                        {
                            var devices = DeviceDtos.FindAll(d => d.IsSelected).Select(dd => new DeviceController() { DeviceID = dd.ID });
                            var users = SelectedSyncUserDtos.Select(u => new User() { UserID = u.ID, Name = u.DisplayName });

                            resultTypes = new DSProxy.DeviceServiceClient().SyncDeviceUsers(devices.ToArray(), DSProxy.SyncOption.Unknown, users.ToArray(),out messages);
                        }

                        message = MessageHandler.GenerateDeviceMessage(resultTypes, messages, "同步数据成功！", "同步数据失败！");
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
                var users = _userRepo.QueryUsersForSummaryData(new Hashtable { { "DepartmentID", selectedNode.ID } }).ToList();
                if (users != null && users.Any())
                {
                    users.ForEach(user => DepartmentUserDtos.Add(new ComboBoxItem() { ID = user.UserID, DisplayName = user.Name }));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private ObservableCollection<SelectableItem> GetSourceUserDtos()
        {
            if (SyncUserType == SyncUserType.SyncDeviceToUser)
            {
                return DeviceUserDtos;
            }
            else
            {
                return DepartmentUserDtos;
            }
        }

        private void MoveToSelected()
        {
            var selectedUsers = GetSourceUserDtos().FindAll(u => u.IsSelected);
            if (selectedUsers == null || !selectedUsers.Any()) return;

            selectedUsers.ForEach(u =>
            {
                if (SelectedSyncUserDtos.All(sy => sy.ID != u.ID))
                {
                    SelectedSyncUserDtos.Add(new ComboBoxItem() { ID = u.ID, DisplayName = u.DisplayName, Description = u.Description});
                }
            });
        }

        private void RemoveSelected()
        {
            var selectedUsers = SelectedSyncUserDtos.FindAll(u => u.IsSelected);
            if (selectedUsers == null || !selectedUsers.Any()) return;

            selectedUsers.ForEach(u => SelectedSyncUserDtos.Remove(u));
        }

        private void SelectAllDepartmentUsers()
        {
            var sourceUserDtos = GetSourceUserDtos();
            if (sourceUserDtos == null || sourceUserDtos.Count == 0)
                return;

            sourceUserDtos.ForEach(u =>
            {
                if (SelectedSyncUserDtos.All(sy => sy.ID != u.ID))
                {
                    SelectedSyncUserDtos.Add(new ComboBoxItem() { ID = u.ID, DisplayName = u.DisplayName });
                }
            });
        }

        private void RemoveAllSelectedUsers()
        {
            SelectedSyncUserDtos = new ObservableCollection<SelectableItem>();
        }

        private void QueryDeviceUser()
        {
            if (SelectedDevice == null)
            {
                SendMessage("选择设备不能为空。");
                return;
            }

            if (!string.IsNullOrWhiteSpace(UserCode) && UserCode.ToInt32() == ConvertorExtension.ConvertionFailureValue)
            {
                SendMessage("输入的人员工号值无效。");
                return;
            }

            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                string message = "";
                DSProxy.DeviceUserDto[] deviceUsers = null;

                var controller = await DialogCoordinator.Instance.ShowProgressAsync(this, "查询设备人员", "查询设备人员中，请稍等");
                controller.SetIndeterminate();

                await Task.Run(() =>
                {
                    try
                    {
                        DSProxy.ResultTypes resultTypes;
                        string[] messages;

                        deviceUsers = new DSProxy.DeviceServiceClient().QueryDeviceUsers(SelectedDevice, UserCode, out resultTypes, out messages);
                        message = MessageHandler.GenerateDeviceMessage(resultTypes, messages, "", "查询设备人员失败！");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        message = "查询设备人员失败！";
                    }
                });

                await controller.CloseAsync();

                if (!string.IsNullOrWhiteSpace(message))
                {
                    Messenger.Default.Send(new NotificationMessage(message), Tokens.SyncUserView_ShowNotification);
                }
                else
                {
                    DeviceUserDtos = new ObservableCollection<SelectableItem>();
                    if (deviceUsers != null && deviceUsers.Any())
                    {
                        deviceUsers.OrderBy(x => x.UserCode).ForEach(x =>
                        {
                            var item = new ComboBoxItem() { ID = x.UserCode, DisplayName = x.UserName };
                            var conditions = new Hashtable() { { "UserCode", x.UserCode} };
                            var userInfo = _userRepo.QueryUsersForSummaryData(conditions).FirstOrDefault();
                            if (userInfo != null)
                            {
                                item.Description = userInfo.UserID.ToString();
                            }

                            DeviceUserDtos.Add(item);
                        });
                    }
                }
            });
        }
    }
}
