using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Validator;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class SysRoleViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysRoleRepository _sysRoleRepo = NinjectBinder.GetRepository<ISysRoleRepository>();
        private const Int32 ModuleNodeType = 1;
        private const Int32 ElementNodeType = 2;


        public virtual Int32 RoleID { get; set; }
        public virtual String RoleName { get; set; }
        public virtual String Description { get; set; }
        public virtual String Remark { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual String AuthorizationModuleString { get; set; }
        public String Title { get; set; }
        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public ObservableCollection<TriStateTreeViewNode> TreeViewSource
        {
            get { return BuildTreeViewSource(); }
        }

        public SysRoleViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
        }

        private void Save()
        {
            string message = "";
            try
            {
                var coreModel = Mapper.Map<SysRole>(this);
                var validator = NinjectBinder.GetValidator<SysRoleValidator>();
                var results = validator.Validate(coreModel);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                if (RoleID == 0)
                {
                    Status = GeneralStatus.Enabled;
                    coreModel.CreateDate = DateTime.Now;
                    coreModel.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    coreModel = _sysRoleRepo.Insert(coreModel);
                    RoleID = coreModel.RoleID;
                    message = "增加成功!";
                }
                else
                {
                    coreModel.UpdateDate = DateTime.Now;
                    coreModel.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    _sysRoleRepo.Update(coreModel);
                    message = "修改成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update sys role fails.", ex);
                message = "保存失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.SysRoleView_Close);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.SysRoleView_ShowNotification);
        }

        private ObservableCollection<TriStateTreeViewNode> BuildTreeViewSource()
        {
            var treeViewRoots = new ObservableCollection<TriStateTreeViewNode>();

            var rootModules = SysPermissionProvider.GetInstance().AllSysModules.FindAll(d => d.Parent == null);
            foreach (var module in rootModules)
            {
                var rootNode = new TriStateTreeViewNode(null) { ID = module.ModuleID, Name = module.ModuleName, NodeType = ModuleNodeType };
                BindChildrenNodes(rootNode);
                treeViewRoots.Add(rootNode);
            }

            return treeViewRoots;
        }

        private void BindChildrenNodes(TriStateTreeViewNode currentNode)
        {
            var allModules = SysPermissionProvider.GetInstance().AllSysModules;
            var moduleChildren = allModules.FindAll(x => (x.Parent != null && x.Parent.ModuleID == currentNode.ID));
            foreach (var element in moduleChildren)
            {
                var subNode = new TriStateTreeViewNode(currentNode) { ID = element.ModuleID, Name = element.ModuleName, NodeType = ModuleNodeType };
                BindChildrenNodes(subNode);
                currentNode.ChildrenNodes.Add(subNode);
            }

            var allElements = SysPermissionProvider.GetInstance().AllSysModuleElements;
            var elementChildren = allElements.FindAll(x => x.ModuleID == currentNode.ID);
            foreach (var element in elementChildren)
            {
                var subNode = new TriStateTreeViewNode(currentNode) { ID = element.ElementID, Name = element.ElementName, NodeType = ElementNodeType };
                currentNode.ChildrenNodes.Add(subNode);
            }
        }

        public void BindPermissionsToTreeView(List<SysRolePermission> permissions)
        {
            TreeViewSource.ForEach(x => x.IsChecked = false);

            var nodes = new List<TriStateTreeViewNode>();
            TreeViewSource.ForEach(x => nodes.AddRange(x.ConvertToList()));
            foreach (var permission in permissions.FindAll(x => x.ElementInfo != null))
            {
                var node = nodes.FirstOrDefault(x => x.NodeType == ElementNodeType && x.ID == permission.ElementInfo.ElementID);
                node.IsChecked = true;
            }
        }

        public List<SysRolePermission> GetPermissionsFromUI()
        {
            var result = new List<SysRolePermission>();

            var nodes = new List<TriStateTreeViewNode>();
            TreeViewSource.ForEach(x => nodes.AddRange(x.ConvertToList()));

            var allElements = SysPermissionProvider.GetInstance().AllSysModuleElements;
            var elements = nodes.FindAll(x => x.NodeType == ElementNodeType && x.IsChecked == true);
            foreach (var element in elements)
            {
                var permission = new SysRolePermission()
                {
                    RoleID = RoleID,
                    ElementInfo = allElements.FirstOrDefault(x => x.ElementID == element.ID),
                };
                result.Add(permission);
            }

            var allModules = SysPermissionProvider.GetInstance().AllSysModules;
            var modules = nodes.FindAll(x => x.NodeType == ModuleNodeType && x.IsChecked == true);
            foreach (var module in modules)
            {
                var permission = new SysRolePermission()
                {
                    RoleID = RoleID,
                    ModuleInfo = allModules.FirstOrDefault(x => x.ModuleID == module.ID),
                };
                result.Add(permission);
            }

            return result;
        }
    }
}
