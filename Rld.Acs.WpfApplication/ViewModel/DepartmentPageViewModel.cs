using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DepartmentPageViewModel : ViewModelBase
    {
        public List<TreeViewNode> TreeViewRoots { get; set; }
        public List<Department> AuthorizationDepartments { get; set; }
        private IDepartmentRepository _departmentRepository = NinjectBinder.GetRepository<IDepartmentRepository>();

        public DepartmentPageViewModel()
        {
            TreeViewRoots = GetTreeViewSource();
        }

        public List<TreeViewNode> GetTreeViewSource()
        {
            List<TreeViewNode> treeViewRoots = new List<TreeViewNode>();
            AuthorizationDepartments = _departmentRepository.Query(new Hashtable()).ToList();

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
