using System;
using System.Collections;
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
using GalaSoft.MvvmLight.Threading;
using log4net;
using MahApps.Metro.Controls.Dialogs;
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
using DSProxy = Rld.Acs.WpfApplication.DeviceProxy;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SyncDepartmentViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand<TreeViewNode> SelectedTreeNodeChangedCmd { get; private set; }
        public RelayCommand SelectDepartmentsCmd { get; private set; }
        public RelayCommand RemoveSelectedDepartmentCmd { get; private set; }
        public RelayCommand SelectAllDepartmentsCmd { get; private set; }
        public RelayCommand RemoveAllSelectedDepartmentsCmd { get; private set; }


        public List<DeviceController> AuthorizationDevices
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices; }
        }
        public List<Department> AuthorizationDepartments
        {
            get { return ApplicationManager.GetInstance().AuthorizationDepartments; }
        }

        public ObservableCollection<SelectableItem> DeviceDtos { get; set; }

        public List<TreeViewNode> TreeViewSource
        {
            get { return BuildTreeViewSource(); }
        }

        private ObservableCollection<SelectableItem> _selectedSyncDepartmentDtos;

        public ObservableCollection<SelectableItem> SelectedSyncDepartmentDtos
        {
            get { return _selectedSyncDepartmentDtos; }
            set
            {
                if (_selectedSyncDepartmentDtos != value)
                {
                    _selectedSyncDepartmentDtos = value;
                    RaisePropertyChanged();
                }
            }
        }
        public TreeViewNode SelectedNode { get; set; }


        public SyncDepartmentViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(ShowUserBySelectedDepartmentNode);
            SelectDepartmentsCmd = new RelayCommand(SelectDepartments);
            RemoveSelectedDepartmentCmd = new RelayCommand(RemoveSelectedDepartment);
            SelectAllDepartmentsCmd = new RelayCommand(SelectAllDepartments);
            RemoveAllSelectedDepartmentsCmd = new RelayCommand(RemoveAllSelectedDepartments);

            SelectedSyncDepartmentDtos = new ObservableCollection<SelectableItem>();

            var dtos = AuthorizationDevices.Select(x => new ListBoxItem {ID = x.DeviceID, DisplayName = x.Name});
            DeviceDtos = new ObservableCollection<SelectableItem>(dtos);
        }


        private void Save()
        {
            var validator = NinjectBinder.GetValidator<SyncDepartmentViewModelValidator>();
            var results = validator.Validate(this);
            if (!results.IsValid)
            {
                var message = string.Join("\n", results.Errors);
                SendMessage(message);
                return;
            }

            string question = "确定同步数据吗？";
            Messenger.Default.Send(new NotificationMessageAction(this, question, SyncData), Tokens.SyncDepartmentView_ShowQuestion);
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
                        var devices = DeviceDtos.FindAll(d => d.IsSelected).Select(dd => new DeviceController() { DeviceID = dd.ID });
                        var departments = SelectedSyncDepartmentDtos.Select(u => new Department() { DepartmentID = u.ID });

                        string[] messages;
                        DSProxy.ResultTypes resultTypes = new DSProxy.DeviceServiceClient().SyncDepartmentUsers(departments.ToArray(), devices.ToArray(), out messages);

                        message = MessageHandler.GenerateDeviceMessage(resultTypes, "同步数据成功！", "同步数据失败！");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        message = "同步数据失败！";
                    }
                });

                await controller.CloseAsync();

                SendMessage(message);
            });
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.SyncDepartmentView_ShowNotification);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseSyncDepartmentView);
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

                SelectedNode = selectedNode;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void SelectDepartments()
        {
            if (SelectedNode == null) return;

            if (SelectedSyncDepartmentDtos.All(sy => sy.ID != SelectedNode.ID))
            {
                SelectedSyncDepartmentDtos.Add(new ComboBoxItem() { ID = SelectedNode.ID, DisplayName = SelectedNode.Name });
            }
        }

        private void RemoveSelectedDepartment()
        {
            var selectedDepartments = SelectedSyncDepartmentDtos.FindAll(u => u.IsSelected);
            if (selectedDepartments == null || !selectedDepartments.Any()) return;

            selectedDepartments.ForEach(u => SelectedSyncDepartmentDtos.Remove(u));
        }

        private void SelectAllDepartments()
        {
            AuthorizationDepartments.ForEach(t =>
            {
                if (SelectedSyncDepartmentDtos.All(sy => sy.ID != t.DepartmentID))
                {
                    SelectedSyncDepartmentDtos.Add(new ComboBoxItem() { ID = t.DepartmentID, DisplayName = t.Name });
                }
            });
        }

        private void RemoveAllSelectedDepartments()
        {
            SelectedSyncDepartmentDtos = new ObservableCollection<SelectableItem>();
        }
    }
}
