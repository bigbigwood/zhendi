using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Rld.Acs.WpfApplication.View.Pages
{
    public class BasePage : Page
    {
        public virtual void ShowQuestionAndAction(NotificationMessageAction msg, string title)
        {
            MessageBoxSingleton.Instance.ShowYesNo(msg.Notification, title, msg.Execute);
        }

        public virtual void ShowMessage(NotificationMessage msg)
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
