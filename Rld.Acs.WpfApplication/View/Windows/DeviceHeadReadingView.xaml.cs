using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class DeviceHeadReadingView : BaseWindow
    {
        public DeviceHeadReadingView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseDeviceHeadReadingView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DeviceHeadReadingView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
