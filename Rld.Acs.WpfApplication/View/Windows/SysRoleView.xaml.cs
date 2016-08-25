using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class SysRoleView : BaseWindow
    {
        public SysRoleView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.SysRoleView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.SysRoleView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
