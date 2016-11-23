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
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DepartmentPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDepartmentRepository _departmentRepository = NinjectBinder.GetRepository<IDepartmentRepository>();

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

        public List<Department> AuthorizationDepartments
        {
            get { return ApplicationManager.GetInstance().AuthorizationDepartments; }
        }
        public List<DeviceController> AuthorizationDevices
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices; }
        }
        public List<DeviceRole> AuthorizationDeviceRoles
        {
            get { return ApplicationManager.GetInstance().AuthorizationDeviceRoles; }
        }
        public DepartmentDetailViewModel SelectedDepartmentDetailViewModel { get; set; }
        public Boolean HasSelectedDepartment { get { return SelectedDepartmentDetailViewModel != null; } }

        public DepartmentPageViewModel()
        {
            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(UpdateDepartmentDetailViewModel);
            AddDepartmentCmd = new AuthCommand(AddDepartment);
            ModifyDepartmentCmd = new AuthCommand(ModifyDepartment);
            DeleteDepartmentCmd = new AuthCommand(ProcessDeleteDepartmentCmd);
            SyncDataCmd = new AuthCommand(SyncDepartment);

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
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.DepartmentPage_ShowNotification);
                    return;
                }

                string assiciationErrorMessage = "";
                if (AuthorizationDepartments.Any(d => d.Parent != null && d.Parent.DepartmentID == SelectedDepartmentDetailViewModel.CurrentDepartment.DepartmentID))
                {
                    assiciationErrorMessage += LanguageManager.GetLocalizationResource(Resource.MSG_CannotDeleteDeptBecauseOfSubDept);
                }

                var userRepo = NinjectBinder.GetRepository<IUserRepository>();
                var departmentUsers = userRepo.Query(new Hashtable() { { "DepartmentID", SelectedDepartmentDetailViewModel.CurrentDepartment.DepartmentID } });
                if (departmentUsers.Any())
                {
                    assiciationErrorMessage += LanguageManager.GetLocalizationResource(Resource.MSG_CannotDeleteDeptBecauseOfExistStaff);
                }

                if (!string.IsNullOrWhiteSpace(assiciationErrorMessage))
                {
                    Messenger.Default.Send(new NotificationMessage(assiciationErrorMessage), Tokens.DepartmentPage_ShowNotification);
                    return;
                }

                string question = string.Format(LanguageManager.GetLocalizationResource(Resource.MSG_DoUWantToDeleteObject), SelectedDepartmentDetailViewModel.DepartmentName);
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
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteSuccessfully);

                    AuthorizationDepartments.Remove(SelectedDepartmentDetailViewModel.CurrentDepartment);
                    TreeViewSource = BuildTreeViewSource();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteFail);
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DepartmentPage_ShowNotification);
            });
        }

        private void SyncDepartment()
        {
            try
            {
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = new SyncDepartmentViewModel(),
                    WindowType = "SyncDepartmentView",

                }, Tokens.OpenDepartmentView);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void ModifyDepartment()
        {
            try
            {
                if (SelectedDepartmentDetailViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.DepartmentPage_ShowNotification);
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
                var departmentDetailViewModel = new DepartmentDetailViewModel();
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
