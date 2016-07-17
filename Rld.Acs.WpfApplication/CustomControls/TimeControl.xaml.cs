using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rld.Acs.WpfApplication
{
	/// <summary>
	/// Interaction logic for TimeControl.xaml
	/// </summary>
	public partial class TimeControl : UserControl
	{
		public TimeControl()
		{
			this.InitializeComponent();
		}

        public static readonly DependencyProperty DurationsProperty =
            DependencyProperty.Register("Durations", typeof(IEnumerable), typeof(TimeControl),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DurationsPropertyChangedCallback)));

        [Description("")]
        [Category("")]
        public IEnumerable Durations
        {
            get
            {
                return (IEnumerable)this.GetValue(DurationsProperty);
            }
            set
            {
                this.SetValue(DurationsProperty, value);
            }
        }

        private static void DurationsPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null && sender is TimeControl)
            {
                TimeControl clock = sender as TimeControl;

                ReDraw((IEnumerable)arg.NewValue);
            }
        }

        private static void ReDraw(IEnumerable newDurations)
	    {
	        
	    }
	}
}