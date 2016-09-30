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
    public partial class InHouseUserView : BaseWindow
    {
        public InHouseUserView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.UserDeviceAuthView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
        }

        private void BtnQuit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
