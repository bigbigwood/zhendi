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
    public partial class TimeSegmentPage : BasePage
    {
        public TimeSegmentPage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenTimeSegmentView, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.TimeSegmentPage_ShowNotification, new Action<NotificationMessage>((msg) => { ShowNotification(msg); }));
            Messenger.Default.Register(this, Tokens.TimeSegmentPage_ShowQuestion, new Action<NotificationMessageAction>((msg) => { ProcessShowNotificationAction(msg); }));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }

        private void ProcessShowNotificationAction(NotificationMessageAction msg)
        {
            MessageBoxSingleton.Instance.ShowYesNo(msg.Notification, "删除时间段", () => { msg.Execute(); });
        }

        private void ShowNotification(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
                MessageBoxSingleton.Instance.ShowDialog(msg.Notification, "");
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            var view = new TimeSegmentView { DataContext = msg.DataContext };
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();
        }
    }
}
