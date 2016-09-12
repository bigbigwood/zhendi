using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class TimeZoneDashboardView : BaseWindow
    {
        public TimeZoneDashboardView()
        {
            InitializeComponent();
        }

        private void BtnQuit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
