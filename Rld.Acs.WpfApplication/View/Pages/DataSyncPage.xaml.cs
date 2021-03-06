﻿using System;
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
    public partial class DataSyncPage : BasePage
    {
        public DataSyncPage()
        {
            InitializeComponent();
            Messenger.Default.Register(this, Tokens.DataSyncPage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
        }
    }
}
