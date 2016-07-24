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
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
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
        private List<TreeViewNode> _treeViewSource;

        public List<TreeViewNode> TreeViewSource
        {
            get { return _treeViewSource; }
            set
            {
                _treeViewSource = value;
                RaisePropertyChanged("TreeViewSource");
            }
        }
        public List<Department> AuthorizationDepartments { get; set; }
        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<DeviceRole> AuthorizationDeviceRoles { get; set; }
        public DepartmentDetailViewModel SelectedDepartmentDetailViewModel { get; set; }
        public Boolean HasSelectedDepartment { get { return SelectedDepartmentDetailViewModel != null; } }
        private IDepartmentRepository _departmentRepository = NinjectBinder.GetRepository<IDepartmentRepository>();


        public DepartmentPageViewModel()
        {
            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(UpdateDepartmentDetailViewModel);
            AddDepartmentCmd = new RelayCommand(AddDepartment);
            ModifyDepartmentCmd = new RelayCommand(ModifyDepartment);
            DeleteDepartmentCmd = new RelayCommand(ProcessDeleteDepartmentCmd);
            SyncDataCmd = new RelayCommand(SyncDepartment);

            AuthorizationDepartments = ApplicationManager.GetInstance().AuthorizationDepartments;
            AuthorizationDevices = ApplicationManager.GetInstance().AuthorizationDevices;
            AuthorizationDeviceRoles = ApplicationManager.GetInstance().AuthorizationDeviceRoles;

            TreeViewSource = BuildTreeViewSource();
        }

        private void UpdateDepartmentDetailViewModel(TreeViewNode selectedNode)
        {
            try
            {
                if (selectedNode.ID == -1)
                    return;

                var dept = AuthorizationDepartments.FirstOrDefault(d => d.DepartmentID == selectedNode.ID);
                var parentDept = AuthorizationDepartments.FirstOrDefault(d => dept.Parent != null && d.DepartmentID == dept.Parent.DepartmentID);

                SelectedDepartmentDetailViewModel = new DepartmentDetailViewModel()
                {
                    ID = dept.DepartmentID,
                    DepartmentName = dept.Name,
                    DepartmentCode = dept.DepartmentCode,
                    OwnedDevices = dept.DeviceAssociations.ToList(),
                    DeviceRole = AuthorizationDeviceRoles.First(r => r.DeviceRoleID == dept.DeviceRoleID),
                    ParentDepartment = parentDept,
                    CurrentDepartment = dept,
                };

                RaisePropertyChanged(null);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
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

                if (AuthorizationDepartments.Any(d => d.Parent != null && d.Parent.DepartmentID == SelectedDepartmentDetailViewModel.CurrentDepartment.DepartmentID))
                {
                    Messenger.Default.Send(new NotificationMessage("选中部门存在子部门，请先删除所属子部门!"), Tokens.DepartmentPage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除部门:{0}吗？", SelectedDepartmentDetailViewModel.DepartmentName);
                Messenger.Default.Send(new NotificationMessageAction(this, question, DeleteDepartment), Tokens.DepartmentPage_ShowQuestion);

                TreeViewSource = BuildTreeViewSource();
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

                    AuthorizationDepartments.Remove(SelectedDepartmentDetailViewModel.CurrentDepartment);
                    TreeViewSource = BuildTreeViewSource();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除部门失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DepartmentPage_ShowNotification);
            });
        }

        private void SyncDepartment()
        {
            Messenger.Default.Send(new NotificationMessage("此功能还未实现..."), Tokens.DepartmentPage_ShowNotification);
            return;
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

                TreeViewSource = BuildTreeViewSource();
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
                var departmentDetailViewModel = new DepartmentDetailViewModel()
                {
                    AuthorizationDepartments = AuthorizationDepartments,
                    AuthorizationDeviceRoles = AuthorizationDeviceRoles,
                    AuthorizationDevices = AuthorizationDevices,
                };

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = departmentDetailViewModel
                
                }, Tokens.OpenDepartmentView);

                if (departmentDetailViewModel.CurrentDepartment.DepartmentID != 0)
                {
                    AuthorizationDepartments.Add(departmentDetailViewModel.CurrentDepartment);
                }
                TreeViewSource = BuildTreeViewSource();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
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
    }
}
