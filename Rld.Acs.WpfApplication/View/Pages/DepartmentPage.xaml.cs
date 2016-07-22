using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View.Windows;
using Rld.Acs.WpfApplication.Views;

namespace Rld.Acs.WpfApplication.View.Pages
{
    /// <summary>
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : BasePage
    {
        public DepartmentPage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenDepartmentView, OpenDepartmentView);
            Messenger.Default.Register(this, Tokens.DepartmentPage_ShowNotification, new Action<NotificationMessage>(ShowNotification));
            Messenger.Default.Register(this, Tokens.DepartmentPage_ShowQuestion, new Action<NotificationMessageAction>(ProcessShowNotificationAction));
        }

        private void ProcessShowNotificationAction(NotificationMessageAction msg)
        {
            MessageBoxSingleton.Instance.ShowYesNo(msg.Notification, "删除部门", msg.Execute);
        }

        private void OpenDepartmentView(OpenWindowMessage msg)
        {
            var view = new DepartmentView {DataContext = msg.DataContext};
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();
        }
    }
}
