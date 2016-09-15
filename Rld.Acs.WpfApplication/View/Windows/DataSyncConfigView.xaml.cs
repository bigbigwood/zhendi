using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class DataSyncConfigView : BaseWindow
    {
        public DataSyncConfigView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.DataSyncView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DataSyncView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
