using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for DepartmentView.xaml
    /// </summary>
    public partial class DepartmentView : BaseWindow
    {
        public DepartmentView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseDepartmentView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DepartmentView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
