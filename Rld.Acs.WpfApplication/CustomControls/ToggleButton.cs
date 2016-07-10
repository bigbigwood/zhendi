using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rld.Acs.WpfApplication.CustomerControl
{
    public class ToggleButton : ImageButton, INotifyPropertyChanged
    {
        private bool expand;

        public bool Expand
        {
            get
            {
                return expand;
            }
            set
            {
                expand = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Expand"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
