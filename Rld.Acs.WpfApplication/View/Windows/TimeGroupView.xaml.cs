using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class TimeGroupView : BaseWindow
    {
        public TimeGroupView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseTimeGroupView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.TimeGroupView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
