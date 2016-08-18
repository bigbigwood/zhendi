﻿using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class DeviceRolePermissionView : BaseWindow
    {
        public DeviceRolePermissionView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseDeviceRolePermissionView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DeviceRolePermissionView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
