using System;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class SyncDepartmentView : BaseWindow
    {
        public SyncDepartmentView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseSyncDepartmentView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.SyncDepartmentView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
            Messenger.Default.Register(this, Tokens.SyncDepartmentView_ShowQuestion, new Action<NotificationMessageAction>(msg => ShowQuestionAndAction(msg, "同步数据")));
        }
    }
}
