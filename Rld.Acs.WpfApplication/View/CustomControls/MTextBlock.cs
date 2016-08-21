using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rld.Acs.WpfApplication.View.CustomControls
{
    public class MTextBlock : TextBlock
    {
        static MTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTextBlock), new FrameworkPropertyMetadata(typeof(MTextBlock)));
        }

        public static readonly DependencyProperty MTextProperty = DependencyProperty.Register
            ("MText", typeof(string), typeof(MTextBlock), new FrameworkPropertyMetadata(String.Empty));

        public string MText
        {
            get { return (string)GetValue(MTextProperty); }
            set { SetValue(MTextProperty, value); }
        }
    }
}
