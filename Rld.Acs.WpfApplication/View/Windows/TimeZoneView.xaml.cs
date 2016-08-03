﻿using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class TimeZoneView : BaseWindow
    {
        public TimeZoneView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseTimeZoneView, new Action<NotificationMessage>(ProcessCloseViewMessage));
        }
    }
}