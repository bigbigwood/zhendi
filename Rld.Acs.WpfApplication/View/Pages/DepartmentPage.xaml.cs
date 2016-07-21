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
using MahApps.Metro.Controls.Dialogs;
using Rld.Acs.WpfApplication.Extension;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Views;

namespace Rld.Acs.WpfApplication.Pages
{
    /// <summary>
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : Page
    {
        public DepartmentPage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenDepartmentView, OpenDepartmentView);
            Messenger.Default.Register(this, Tokens.DepartmentPage_ShowNotification, new Action<NotificationMessage>((msg) => { ShowNotification(msg); }));
            Messenger.Default.Register(this, Tokens.DepartmentPage_ShowQuestion, new Action<NotificationMessageAction>((msg) => { ProcessShowNotificationAction(msg); }));
        }

        private void DeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {


            //string questionMessage = string.Format("确定删除部门: \"{0}\" 吗？", this.departmentDetailView.TextBlock_DepartmentName.Text);
            //var message = new NotificationMessageAction(Tokens.DepartmentPage_DeleteDepartmentAction,
            //    ShowDepartmentDeletedDoneMessage);

            //MessageBoxSingleton.Instance.ShowYesNo(questionMessage, "删除部门",
            //    () => Messenger.Default.Send(message, Tokens.DepartmentPage_DeleteDepartmentAction));
        }

        private void ProcessShowNotificationAction(NotificationMessageAction msg)
        {
            MessageBoxSingleton.Instance.ShowYesNo(msg.Notification, "删除部门", () => { msg.Execute(); });
        }

        private void ShowNotification(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
                MessageBoxSingleton.Instance.ShowDialog(msg.Notification, "");
        }

        private void OpenDepartmentView(OpenWindowMessage msg)
        {
            var view = new DepartmentView {DataContext = msg.DataContext};
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }
    }
}
