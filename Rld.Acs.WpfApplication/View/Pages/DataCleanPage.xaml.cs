using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View.Windows;

namespace Rld.Acs.WpfApplication.View.Pages
{
    /// <summary>
    /// Interaction logic for TimeGroupPage.xaml
    /// </summary>
    public partial class DataCleanPage : BasePage
    {
        public DataCleanPage()
        {
            InitializeComponent();
            Messenger.Default.Register(this, Tokens.DataCleanPage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
        }
    }
}
