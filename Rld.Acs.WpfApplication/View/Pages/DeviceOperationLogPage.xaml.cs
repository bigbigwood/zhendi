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
    public partial class DeviceOperationLogPage : BasePage
    {
        public DeviceOperationLogPage()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.DeviceOperationLogPage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
        }

    }
}
