using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using log4net;

namespace Rld.Acs.WpfApplication
{
	/// <summary>
	/// Interaction logic for TimeControl.xaml
	/// </summary>
	public partial class TimeControl : UserControl
	{
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public TimeControl()
		{
			this.InitializeComponent();

            Init();
		}

        private Style disabledBorderStyle;
        private Style enabledBorderStype;
        private Dictionary<Int32, Border> borderDictionary = new Dictionary<int, Border>();

        private void Init()
        {
            disabledBorderStyle = (Style)FindResource("GrayBorderStyle");
            enabledBorderStype = (Style)FindResource("GreenBorderStyle");

            for (int i = 1; i < 97; i++)
            {
                var borderName = String.Format("B{0}", i);
                var myBorder = (Border)this.FindName(borderName);
                borderDictionary.Add(i, myBorder);
            }
        }

        public readonly static DependencyProperty DurationsSourceProperty =
            DependencyProperty.Register("DurationsSource", typeof(IEnumerable), typeof(TimeControl),
            new FrameworkPropertyMetadata(new ObservableCollection<string>(), 
                new PropertyChangedCallback(DurationsPropertyChangedCallback)));

        public IEnumerable DurationsSource
        {
            get { return GetValue(DurationsSourceProperty) as IEnumerable; }
            set { SetValue(DurationsSourceProperty, value); }
        }

        private static void DurationsPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null && sender is TimeControl)
            {
                Log.Info("fire TimeControl callback");
                if (arg.NewValue != arg.OldValue)
                {
                    TimeControl timeControl = sender as TimeControl;
                    timeControl.ReDraw(arg.NewValue as ObservableCollection<string>); 
                }
            }
        }

        private void ReDraw(ObservableCollection<string> newDurations)
	    {
            Log.Info("fire TimeControl redraw");

            foreach (var border in borderDictionary.Values.Where(b => b.Style == enabledBorderStype))
            {
                border.Style = disabledBorderStyle;
            }

            if (newDurations == null || newDurations.Count == 0) 
                return;

            foreach (var newDuration in newDurations)
            {
                var enabledBorders = CalculateEnabledBorder(newDuration);
                if (enabledBorders != null)
                {
                    enabledBorders.ForEach(b => b.Style = enabledBorderStype);
                }
            }
	    }

        /// <summary>
        /// Expect format: "09:00-12:00"
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
	    private List<Border> CalculateEnabledBorder(string duration)
	    {
            int beginHour = int.Parse(duration.Substring(0,2));
            int beginMinute = int.Parse(duration.Substring(3, 2));
            int endHour = int.Parse(duration.Substring(6, 2));
            int endMinute = int.Parse(duration.Substring(9, 2));

            //去除尾数
            //int beginBorderIndex = (beginHour * 60 + beginMinute) / 15 + 1;
            //int endBorderIndex = (endHour * 60 + endMinute) / 15;

            //四舍五入
            int beginBorderIndex = (int)((float)(beginHour * 60 + beginMinute) / 15 + 0.5) + 1;
            int endBorderIndex = (int)((float)(endHour * 60 + endMinute) / 15 + 0.5);

            return borderDictionary.Keys.Where(k => k >= beginBorderIndex && k <= endBorderIndex).
                Select(index => borderDictionary[index]).ToList();
	    }

	}
}