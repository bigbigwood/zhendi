using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class DeviceRoleView : BaseWindow
    {
        public DeviceRoleView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseDeviceRoleView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DeviceRoleView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
