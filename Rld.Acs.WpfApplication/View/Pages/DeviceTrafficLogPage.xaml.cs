using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View.Windows;

namespace Rld.Acs.WpfApplication.View.Pages
{
    /// <summary>
    /// Interaction logic for TimeSegmentPage.xaml
    /// </summary>
    public partial class DeviceTrafficLogPage : BasePage
    {
        public DeviceTrafficLogPage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.DeviceTrafficLogPage_OpenExportView, OpenDepartmentView);
            Messenger.Default.Register(this, Tokens.DeviceTrafficLogPage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
        }

        private void OpenDepartmentView(OpenWindowMessage msg)
        {
            var view = new ExportFileView() { DataContext = msg.DataContext };
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();
        }
    }
}
