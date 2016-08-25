using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace Rld.Acs.WpfApplication.Models
{
    public class TriStateTreeViewNode : ViewModelBase
    {
        private string _name;
        private bool? _isChecked;
        private bool reentrancyCheck = false;
        private TriStateTreeViewNode parentNode;

        private ObservableCollection<TriStateTreeViewNode> _childrenNodes = null;


        public Int32 ID { get; set; }
        public Int32 NodeType { get; set; }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }
        public bool? IsChecked
        {
            get
            {
                return this._isChecked;
            }
            set
            {
                if (this._isChecked != value)
                {
                    if (reentrancyCheck)
                        return;
                    this.reentrancyCheck = true;
                    this._isChecked = value;
                    this.UpdateCheckState();
                    RaisePropertyChanged();
                    this.reentrancyCheck = false;
                }
            }
        }

        public ObservableCollection<TriStateTreeViewNode> ChildrenNodes
        {
            get
            {
                if (this._childrenNodes == null)
                {
                    this._childrenNodes = new ObservableCollection<TriStateTreeViewNode>();
                }
                return this._childrenNodes;
            }
        }

        public TriStateTreeViewNode(TriStateTreeViewNode parent)
        {
            this.parentNode = parent;
        }

        private void UpdateCheckState()
        {
            // update all children:
            if (this.ChildrenNodes.Count != 0)
            {
                this.UpdateChildrenCheckState();
            }
            //update parent item
            if (this.parentNode != null)
            {
                bool? parentIsChecked = this.parentNode.DetermineCheckState();
                this.parentNode.IsChecked = parentIsChecked;

            }
        }

        private void UpdateChildrenCheckState()
        {
            foreach (var item in this.ChildrenNodes)
            {
                if (this.IsChecked != null)
                {
                    item.IsChecked = this.IsChecked;
                }
            }
        }

        private bool? DetermineCheckState()
        {
            bool allChildrenChecked = this.ChildrenNodes.Count(x => x.IsChecked == true) == this.ChildrenNodes.Count;
            if (allChildrenChecked)
            {
                return true;
            }

            bool allChildrenUnchecked = this.ChildrenNodes.Count(x => x.IsChecked == false) == this.ChildrenNodes.Count;
            if (allChildrenUnchecked)
            {
                return false;
            }

            return null;
        }

        public List<TriStateTreeViewNode> ConvertToList()
        {
            List<TriStateTreeViewNode> result= new List<TriStateTreeViewNode>();
            result.Add(this);

            foreach (var node in this.ChildrenNodes)
            {
                result.AddRange(node.ConvertToList());
            }

            return result;
        }
    }
}
