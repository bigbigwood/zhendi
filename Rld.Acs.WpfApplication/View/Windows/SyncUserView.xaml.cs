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
    public partial class SyncUserView : BaseWindow
    {
        public SyncUserView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseSyncUserView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.SyncUserView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
