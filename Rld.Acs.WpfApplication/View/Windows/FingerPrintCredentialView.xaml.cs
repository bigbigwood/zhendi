using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class FingerPrintCredentialView : BaseWindow
    {
        public FingerPrintCredentialView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.UserAuthenticationView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
        }
    }
}
