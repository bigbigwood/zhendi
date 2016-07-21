using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View;
using Rld.Acs.WpfApplication.ViewModel.Views;
using Rld.Acs.WpfApplication.Views;

namespace Rld.Acs.WpfApplication.Pages
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenUserView, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.UserPage_ShowNotification, new Action<NotificationMessage>(ShowNotification));
            Messenger.Default.Register(this, Tokens.UserPage_ShowQuestion, new Action<NotificationMessageAction>(ProcessShowNotificationAction));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }

        private void ProcessShowNotificationAction(NotificationMessageAction msg)
        {
            MessageBoxSingleton.Instance.ShowYesNo(msg.Notification, "删除人员", msg.Execute);
        }

        private void ShowNotification(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
                MessageBoxSingleton.Instance.ShowDialog(msg.Notification, "");
        }
        private void ProcessOpenView(OpenWindowMessage msg)
        {
            var view = new UserView() { DataContext = msg.DataContext };
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();

        }
    }
}
