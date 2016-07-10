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
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DepartmentPageViewModel : ViewModelBase
    {
        public RelayCommand<TreeViewNode> SelectedTreeNodeChangedCmd { get; private set; }
        public RelayCommand AddDepartmentCmd { get; private set; }
        public RelayCommand ModifyDepartmentCmd { get; private set; }
        public RelayCommand SyncDataCmd { get; private set; }

        public TreeViewNode SelectedTreeNode{ get; private set; }
        public List<TreeViewNode> TreeViewRoots { get; set; }
        public List<Department> AuthorizationDepartments { get; set; }
        public DepartmentDetailViewModel DepartmentDetailViewModel { get; set; }
        public Boolean HasSelectedDepartment { get { return DepartmentDetailViewModel != null; } }
        private IDepartmentRepository _departmentRepository = NinjectBinder.GetRepository<IDepartmentRepository>();
        private IDeviceRoleRepository _deviceRoleRepository = NinjectBinder.GetRepository<IDeviceRoleRepository>();
        private IDeviceControllerRepository _deviceControllerRepository = NinjectBinder.GetRepository<IDeviceControllerRepository>();
        private IDepartmentDeviceRepository _departmentDeviceRepository = NinjectBinder.GetRepository<IDepartmentDeviceRepository>();


        public DepartmentPageViewModel()
        {
            Messenger.Default.Register(this, Tokens.DepartmentPage_DeleteDepartmentAction, new Action<NotificationMessageAction>((msg) => { DeleteDepartment(msg); }));

            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(c => UpdateDepartmentDetailViewModel(c));
            //AddDepartmentCmd = new RelayCommand();
            ModifyDepartmentCmd = new RelayCommand(ModifyDepartment);
            //SyncDataCmd = new RelayCommand();

            AuthorizationDepartments = _departmentRepository.Query(new Hashtable()).ToList();
            TreeViewRoots = GetTreeViewSource();
        }

        private void UpdateDepartmentDetailViewModel(TreeViewNode selectedNode)
        {
            var dept = AuthorizationDepartments.FirstOrDefault(d => d.DepartmentID == selectedNode.ID);

            DepartmentDetailViewModel = new DepartmentDetailViewModel()
            {
                ID = dept.DepartmentID,
                DepartmentName = dept.Name,
                DepartmentCode = dept.DepartmentCode,
                DeviceRole = dept.DeviceRole,
                StuffCount = 10,
            };

            DepartmentDetailViewModel.DeviceRoleList = _deviceRoleRepository.Query(new Hashtable { { "Status", 1 } }).ToList();
            DepartmentDetailViewModel.DeviceList = _deviceControllerRepository.Query(new Hashtable { { "Status", 1 } }).ToList();
            DepartmentDetailViewModel.OwnedDevices = _departmentDeviceRepository.Query(new Hashtable { { "DepartmentID", dept.DepartmentID }, { "Status", 1 } }).ToList();
            DepartmentDetailViewModel.DeviceRole = DepartmentDetailViewModel.DeviceRoleList.First(r => r.DeviceRoleID == dept.DeviceRole.DeviceRoleID);
            DepartmentDetailViewModel.BuildDeviceListBox();
            RaisePropertyChanged("DepartmentDetailViewModel");
            RaisePropertyChanged("HasSelectedDepartment");
        }

        private void DeleteDepartment(NotificationMessageAction msg)
        {
            if (DepartmentDetailViewModel != null)
            {
                if (_departmentRepository.Delete(DepartmentDetailViewModel.ID))
                    msg.Execute();
            }

        }

        private void ModifyDepartment()
        {
            if (DepartmentDetailViewModel != null)
            {
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
            var currentNode = new TreeViewNode() {ID = rootDepartment.DepartmentID, Name = rootDepartment.Name};
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
