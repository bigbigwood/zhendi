using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DepartmentPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RelayCommand<TreeViewNode> SelectedTreeNodeChangedCmd { get; private set; }
        public RelayCommand AddDepartmentCmd { get; private set; }
        public RelayCommand ModifyDepartmentCmd { get; private set; }
        public RelayCommand DeleteDepartmentCmd { get; private set; }
        public RelayCommand SyncDataCmd { get; private set; }

        public TreeViewNode SelectedTreeNode { get; private set; }
        public List<TreeViewNode> TreeViewRoots { get; set; }
        public List<Department> AuthorizationDepartments { get; set; }
        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<DeviceRole> AuthorizationDeviceRoles { get; set; }
        public DepartmentDetailViewModel SelectedDepartmentDetailViewModel { get; set; }
        public Boolean HasSelectedDepartment { get { return SelectedDepartmentDetailViewModel != null; } }
        private IDepartmentRepository _departmentRepository = NinjectBinder.GetRepository<IDepartmentRepository>();
        private IDeviceRoleRepository _deviceRoleRepository = NinjectBinder.GetRepository<IDeviceRoleRepository>();
        private IDeviceControllerRepository _deviceControllerRepository = NinjectBinder.GetRepository<IDeviceControllerRepository>();
        private IDepartmentDeviceRepository _departmentDeviceRepository = NinjectBinder.GetRepository<IDepartmentDeviceRepository>();


        public DepartmentPageViewModel()
        {
            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(UpdateDepartmentDetailViewModel);
            AddDepartmentCmd = new RelayCommand(AddDepartment);
            ModifyDepartmentCmd = new RelayCommand(ModifyDepartment);
            DeleteDepartmentCmd = new RelayCommand(ProcessDeleteDepartmentCmd);
            //SyncDataCmd = new RelayCommand();

            AuthorizationDepartments = _departmentRepository.Query(new Hashtable()).ToList();
            var topDepartment = new Department() { DepartmentID = -1, Name = "总经办" };
            AuthorizationDepartments.Insert(0, topDepartment);
            AuthorizationDepartments.FindAll(d => d.Parent == null && d.DepartmentID != -1).ForEach(d => d.Parent = topDepartment);

            AuthorizationDevices = _deviceControllerRepository.Query(new Hashtable { { "Status", 1 } }).ToList();
            AuthorizationDeviceRoles = _deviceRoleRepository.Query(new Hashtable { { "Status", 1 } }).ToList();

            TreeViewRoots = GetTreeViewSource();
        }

        private void UpdateDepartmentDetailViewModel(TreeViewNode selectedNode)
        {
            if (selectedNode.ID == -1)
                return;

            var dept = AuthorizationDepartments.FirstOrDefault(d => d.DepartmentID == selectedNode.ID);
            var parentDept =
                AuthorizationDepartments.FirstOrDefault(
                    d => dept.Parent != null && d.DepartmentID == dept.Parent.DepartmentID);

            SelectedDepartmentDetailViewModel = new DepartmentDetailViewModel()
            {
                ID = dept.DepartmentID,
                DepartmentName = dept.Name,
                DepartmentCode = dept.DepartmentCode,
                OwnedDevices = dept.DeviceAssociations.ToList(),
                DeviceRole = AuthorizationDeviceRoles.First(r => r.DeviceRoleID == dept.DeviceRoleID),
                ParentDepartment = parentDept,
                CurrentDepartment = dept,
                AuthorizationDepartments = AuthorizationDepartments,
                AuthorizationDeviceRoles = AuthorizationDeviceRoles,
                AuthorizationDevices = AuthorizationDevices,
                StuffCount = 10,
            };

            RaisePropertyChanged("SelectedDepartmentDetailViewModel");
            RaisePropertyChanged("HasSelectedDepartment");
        }

        private void ProcessDeleteDepartmentCmd()
        {
            try
            {
                if (SelectedDepartmentDetailViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择部门!"), Tokens.DepartmentPage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除部门:{0}吗？", SelectedDepartmentDetailViewModel.DepartmentName);
                Messenger.Default.Send(new NotificationMessageAction(this, question, DeleteDepartment), Tokens.DepartmentPage_ShowQuestion);

                // refresh UI
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteDepartment()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _departmentRepository.Delete(SelectedDepartmentDetailViewModel.ID);
                    message = "删除部门成功!";
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除部门失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DepartmentPage_ShowNotification);
            });
        }


        private void ModifyDepartment()
        {
            try
            {
                if (SelectedDepartmentDetailViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择部门!"), Tokens.DepartmentPage_ShowNotification);
                    return;
                }

                Messenger.Default.Send(new OpenWindowMessage() { DataContext = SelectedDepartmentDetailViewModel }, Tokens.OpenDepartmentView);

                // refresh UI
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void AddDepartment()
        {
            try
            {
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = new DepartmentDetailViewModel()
                    {
                        AuthorizationDepartments = AuthorizationDepartments,
                        AuthorizationDeviceRoles = AuthorizationDeviceRoles,
                        AuthorizationDevices = AuthorizationDevices,
                    }
                }, Tokens.OpenDepartmentView);

                // refresh UI
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }


        private List<TreeViewNode> GetTreeViewSource()
        {
            List<TreeViewNode> treeViewRoots = new List<TreeViewNode>();

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
    }
}
