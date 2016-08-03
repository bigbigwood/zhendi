﻿using System;
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
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
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
        public RelayCommand<TreeViewNode> SelectedTreeNodeChangedCmd { get; private set; }

        public List<Department> AuthorizationDepartments { get; set; }
        public TreeViewNode SelectedTreeNode { get; private set; }
        public List<TreeViewNode> TreeViewSource { get; private set; }
        public ObservableCollection<UserViewModel> UserViewModels { get; set; }
        public UserViewModel SelectedUserViewModel { get; set; }

        public UserPageViewModel()
        {
            AddUserCmd = new RelayCommand(AddUser);
            ModifyUserCmd = new RelayCommand(ModifyUser);
            DeleteUserCmd = new RelayCommand(DeleteUser);
            MoveUserCmd = new RelayCommand(MoveUser);
            SyncUserCmd = new RelayCommand(SyncUser);
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

                
                UserViewModels = new ObservableCollection<UserViewModel>();
                var users = _userRepo.Query(new Hashtable { { "DepartmentID", selectedNode.ID } });
                if (users.Any())
                {
                    foreach (var user in users)
                    {
                        UserViewModels.Add(new UserViewModel(user));
                        
                    }
                }

                RaisePropertyChanged(null);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
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

                if (userViewModel.CurrentUser.UserID != 0)
                    UserViewModels.Add(userViewModel);
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
                    Messenger.Default.Send(new NotificationMessage("请先选择用户!"), Tokens.UserPage_ShowNotification);
                    return;
                }

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedUserViewModel

                }, Tokens.OpenUserView);
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
                    Messenger.Default.Send(new NotificationMessage("请先选择用户!"), Tokens.UserPage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除用户:{0}吗？", SelectedUserViewModel.Name);
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
                    message = "删除用户成功!";

                    UserViewModels.Remove(SelectedUserViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除用户失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeGroupPage_ShowNotification);
            });
        }


        private void MoveUser()
        {
            try
            {
                if (SelectedUserViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择用户!"), Tokens.UserPage_ShowNotification);
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
            Messenger.Default.Send(new NotificationMessage("此功能还未实现..."), Tokens.UserPage_ShowNotification);
            return;
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