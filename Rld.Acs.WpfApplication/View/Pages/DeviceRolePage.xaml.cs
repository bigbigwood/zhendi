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
    /// Interaction logic for TimeSegmentPage.xaml
    /// </summary>
    public partial class DeviceRolePage : BasePage
    {
        public DeviceRolePage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenDeviceRoleView, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.DeviceRolePage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
            Messenger.Default.Register(this, Tokens.DeviceRolePage_ShowQuestion, new Action<NotificationMessageAction>((msg) => ShowQuestionAndAction(msg, "删除设备角色")));
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            //var view = new TimeSegmentView { DataContext = msg.DataContext };
            //view.BorderThickness = new Thickness(1);
            //view.GlowBrush = null;
            //view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            //view.ShowDialog();
        }
    }
}
