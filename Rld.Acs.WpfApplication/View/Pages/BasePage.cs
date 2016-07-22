using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Rld.Acs.WpfApplication.View.Pages
{
    public class BasePage : Page
    {
        public virtual void ProcessShowNotificationAction(NotificationMessageAction msg, string Title)
        {
            MessageBoxSingleton.Instance.ShowYesNo(msg.Notification, Title, msg.Execute);
        }

        public virtual void ShowNotification(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
                MessageBoxSingleton.Instance.ShowDialog(msg.Notification, "");
        }

        public virtual void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }
    }
}
