﻿using System;
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
using log4net;
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

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SyncDepartmentViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public RelayCommand<TreeViewNode> SelectedTreeNodeChangedCmd { get; private set; }
        public RelayCommand MoveDepartmentToSelectedCmd { get; private set; }
        public RelayCommand RemoveSelectedDepartmentCmd { get; private set; }
        public RelayCommand SelectAllDepartmentsCmd { get; private set; }
        public RelayCommand RemoveAllSelectedDepartmentsCmd { get; private set; }


        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<Department> AuthorizationDepartments { get; set; }

        public ObservableCollection<SelectableItem> DeviceDtos { get; set; }
        public List<TreeViewNode> TreeViewSource { get; private set; }
        public ObservableCollection<SelectableItem> SelectedSyncDepartmentDtos { get; set; }
        public TreeViewNode SelectedNode { get; set; }


        public SyncDepartmentViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            SelectedTreeNodeChangedCmd = new RelayCommand<TreeViewNode>(ShowUserBySelectedDepartmentNode);
            MoveDepartmentToSelectedCmd = new RelayCommand(MoveDepartmentToSelected);
            RemoveSelectedDepartmentCmd = new RelayCommand(RemoveSelectedDepartment);
            SelectAllDepartmentsCmd = new RelayCommand(SelectAllDepartments);
            RemoveAllSelectedDepartmentsCmd = new RelayCommand(RemoveAllSelectedDepartments);

            DeviceDtos = new ObservableCollection<SelectableItem>();
            SelectedSyncDepartmentDtos = new ObservableCollection<SelectableItem>();

            AuthorizationDevices = ApplicationManager.GetInstance().AuthorizationDevices;
            AuthorizationDevices.ForEach(d => DeviceDtos.Add(new ListBoxItem { ID = d.DeviceID, DisplayName = d.DeviceCode }));

            AuthorizationDepartments = AuthorizationDepartments = ApplicationManager.GetInstance().AuthorizationDepartments;
            TreeViewSource = BuildTreeViewSource();
        }


        private void Save()
        {
            string message = "";
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error("Update user fails.", ex);
                message = "保存人员失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
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

        private void MoveDepartmentToSelected()
        {
            if (SelectedNode == null) return;

            if (SelectedSyncDepartmentDtos.All(sy => sy.ID != SelectedNode.ID))
            {
                SelectedSyncDepartmentDtos.Add(new ComboBoxItem() { ID = SelectedNode.ID, DisplayName = SelectedNode.Name });
            }
            RaisePropertyChanged(null);
        }

        private void RemoveSelectedDepartment()
        {
            var selectedDepartments = SelectedSyncDepartmentDtos.FindAll(u => u.IsSelected);
            if (selectedDepartments == null || !selectedDepartments.Any()) return;

            selectedDepartments.ForEach(u => SelectedSyncDepartmentDtos.Remove(u));
            RaisePropertyChanged(null);
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

            RaisePropertyChanged(null);
        }

        private void RemoveAllSelectedDepartments()
        {
            SelectedSyncDepartmentDtos = new ObservableCollection<SelectableItem>();
            RaisePropertyChanged(null);
        }
    }
}
