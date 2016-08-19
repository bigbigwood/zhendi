using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class DeviceDoorView : BaseWindow
    {
        public DeviceDoorView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseDeviceDoorView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DeviceDoorView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
