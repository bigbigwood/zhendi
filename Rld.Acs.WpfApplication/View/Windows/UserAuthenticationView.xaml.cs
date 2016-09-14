using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class UserAuthenticationView : BaseWindow
    {
        public UserAuthenticationView()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.UserAuthenticationView_Open, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.UserDeviceAuthView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.UserDeviceAuthView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            var view = new FingerPrintCredentialView() { DataContext = msg.DataContext };
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog(); 
        }
    }
}
