using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using Rld.Acs.WpfApplication.Service.Authorization;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class UserPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = NinjectBinder.GetRepository<IUserRepository>();

        public RelayCommand AddUserCmd { get; private set; }
        public RelayCommand ModifyUserCmd { get; private set; }
        public RelayCommand DeleteUserCmd { get; private set; }
        public RelayCommand MoveUserCmd { get; private set; }
        public RelayCommand SyncUserCmd { get; private set; }
        public RelayCommand FilterUserCmd { get; private set; }
        public RelayCommand<string> SearchUserCmd { get; private set; }
        public RelayCommand<TreeViewNode> SelectedTreeNodeChangedCmd { get; private set; }

        public List<Department> AuthorizationDepartments { get; set; }
        public TreeViewNode SelectedTreeNode { get; private set; }
        public List<TreeViewNode> TreeViewSource { get; private set; }
        public ObservableCollection<UserViewModel> UserViewModels { get; set; }
        public UserViewModel SelectedUserViewModel { get; set; }
        public Int32 SelectedDepartmentId { get; set; }
        public Boolean ShowEmployee { get; set; }
        public Boolean ShowVisitor { get; set; }
        public List<User> CurrentDepartmentUsers { get; set; }


        public UserPageViewModel()
        {
            ShowEmployee = true;
            ShowVisitor = true;

            AddUserCmd = new AuthCommand(AddUser);
            ModifyUserCmd = new AuthCommand(ModifyUser);
            DeleteUserCmd = new AuthCommand(DeleteUser);
            MoveUserCmd = new AuthCommand(MoveUser);
            SyncUserCmd = new AuthCommand(SyncUser);
            FilterUserCmd = new RelayCommand(FilterUsers);
            SearchUserCmd = new RelayCommand<string>(SearchUser);
            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(ShowUserBySelectedDepartmentNode);

            UserViewModels = new ObservableCollection<UserViewModel>();
            AuthorizationDepartments = AuthorizationDepartments = ApplicationManager.GetInstance().AuthorizationDepartments;
            TreeViewSource = BuildTreeViewSource();
        }

        private void ShowUserBySelectedDepartmentNode(TreeViewNode selectedNode)
        {
            try
            {
                if (selectedNode.ID == -1)
                    return;

                SelectedDepartmentId = selectedNode.ID;
                CurrentDepartmentUsers = _userRepo.QueryUsersForSummaryData(new Hashtable { { "DepartmentID", selectedNode.ID } }).ToList();

                var filterUesrs = GetFilterUsers();
                var listUserViewModel = filterUesrs.Select(x => new UserViewModel(x));
                UserViewModels = new ObservableCollection<UserViewModel>(listUserViewModel);
                RaisePropertyChanged(null);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void FilterUsers()
        {
            var filterUesrs = GetFilterUsers();
            var listUserViewModel = filterUesrs.Select(x => new UserViewModel(x));
            UserViewModels = new ObservableCollection<UserViewModel>(listUserViewModel);
            RaisePropertyChanged(null);
        }

        private void SearchUser(string keyword)
        {
            CurrentDepartmentUsers = _userRepo.QueryUsersForSummaryData(new Hashtable { { "Keyword", keyword } }).ToList();

            var filterUesrs = GetFilterUsers();
            var listUserViewModel = filterUesrs.Select(x => new UserViewModel(x));
            UserViewModels = new ObservableCollection<UserViewModel>(listUserViewModel);
            RaisePropertyChanged(null);
        }

        private List<User> GetFilterUsers()
        {
            if (ShowEmployee && ShowVisitor)
            {
                return CurrentDepartmentUsers;
            }
            else if (ShowEmployee)
            {
                return CurrentDepartmentUsers.FindAll(x => x.Type == UserType.Employee);
            }
            else if (ShowVisitor)
            {
                return CurrentDepartmentUsers.FindAll(x => x.Type == UserType.Visitor);
            }
            else
            {
                return new List<User>();
            }
        }


        private void AddUser()
        {
            try
            {
                var userViewModel = new UserViewModel(new User());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = userViewModel

                }, Tokens.OpenUserView);

                if (userViewModel.ViewModelAttachment.LastOperationSuccess && userViewModel.DepartmentInfo.DepartmentID == SelectedDepartmentId)
                {
                    UserViewModels.Add(userViewModel);
                    CurrentDepartmentUsers.Add(userViewModel.ViewModelAttachment.CoreModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyUser()
        {
            try
            {
                if (SelectedUserViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择人员!"), Tokens.UserPage_ShowNotification);
                    return;
                }

                var coreModel = _userRepo.GetByKey(SelectedUserViewModel.UserID);
                var viewModel = new UserViewModel(coreModel);
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenUserView);

                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var index = UserViewModels.IndexOf(SelectedUserViewModel);
                    UserViewModels[index] = viewModel;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteUser()
        {
            try
            {
                if (SelectedUserViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择人员!"), Tokens.UserPage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除人员:{0}吗？", SelectedUserViewModel.Name);
                Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmDeleteUser), Tokens.UserPage_ShowQuestion);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void ConfirmDeleteUser()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _userRepo.Delete(SelectedUserViewModel.CurrentUser.UserID);
                    message = "删除人员成功!";

                    UserViewModels.Remove(SelectedUserViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除人员失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.UserPage_ShowNotification);
            });
        }


        private void MoveUser()
        {
            try
            {
                if (SelectedUserViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择人员!"), Tokens.UserPage_ShowNotification);
                    return;
                }

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedUserViewModel,
                    WindowType = "MoveUserView",
                }, Tokens.OpenUserView);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void SyncUser()
        {
            try
            {
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = new SyncUserViewModel(),
                    WindowType = "SyncUserView",

                }, Tokens.OpenUserView);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            //Messenger.Default.Send(new NotificationMessage("此功能还未实现..."), Tokens.UserPage_ShowNotification);
            //return;
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
