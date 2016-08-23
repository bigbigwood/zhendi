using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class SysDictionaryView : BaseWindow
    {
        public SysDictionaryView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseSysDictionaryView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.SysDictionaryView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
