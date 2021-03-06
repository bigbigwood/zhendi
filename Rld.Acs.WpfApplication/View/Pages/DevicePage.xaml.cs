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
    /// Interaction logic for TimeSegmentPage.xaml
    /// </summary>
    public partial class DevicePage : BasePage
    {
        public DevicePage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenDeviceView, ProcessOpenView);
            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.DeviceGroupPage_Open, ProcessOpenDeviceGroupPage);
            Messenger.Default.Register(this, Tokens.DevicePage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
            Messenger.Default.Register(this, Tokens.DevicePage_ShowQuestion, new Action<NotificationMessageAction>((msg) => ShowQuestionAndAction(msg, "删除设备")));
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            var view = new DeviceView() { DataContext = msg.DataContext };
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();
        }

        private void ProcessOpenDeviceGroupPage(OpenWindowMessage msg)
        {
            var view = new DeviceGroupMgntView();
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();
        }
    }
}
