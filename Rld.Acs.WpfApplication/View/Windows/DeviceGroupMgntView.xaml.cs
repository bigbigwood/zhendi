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
    public partial class DeviceGroupMgntView : BaseWindow
    {
        public DeviceGroupMgntView()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.DeviceGroupView_Open, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.DeviceGroupPage_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DeviceGroupPage_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
            Messenger.Default.Register(this, Tokens.DeviceGroupPage_ShowQuestion, new Action<NotificationMessageAction>((msg) => ShowQuestionAndAction(msg, "删除设备组")));
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            var view = new DeviceGroupView() { DataContext = msg.DataContext };
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog(); 
        }
    }
}
