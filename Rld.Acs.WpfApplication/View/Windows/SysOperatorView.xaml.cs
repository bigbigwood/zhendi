using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class SysOperatorView : BaseWindow
    {
        public SysOperatorView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.SysOperatorView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.SysOperatorView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
