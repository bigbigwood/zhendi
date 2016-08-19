using System;

namespace Rld.Acs.WpfApplication.Models
{
    public abstract class SelectableItem
    {
        public virtual Int32 ID { get; set; }
        public Boolean IsEnabled { get; set; }
        public Boolean IsDefault { get; set; }
        public Boolean IsSelected { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as SelectableItem);
        }

        public bool Equals(SelectableItem other)
        {
            if (other == null) return false;
            return ID == other.ID;
        }
    }

    public class ListBoxItem : SelectableItem
    {
    }

    public class ComboBoxItem : SelectableItem
    {
    }

    public class NullableSelectableItem : SelectableItem
    {
        public new Int32? ID { get; set; }
    }
}
