using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class SyncUserView : BaseWindow
    {
        public SyncUserView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseSyncUserView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.SyncUserView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
            Messenger.Default.Register(this, Tokens.SyncUserView_ShowQuestion, new Action<NotificationMessageAction>(msg => ShowQuestionAndAction(msg, "同步数据")));
        }

        private void OnSyncUserTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox.SelectedValue.ToString() == SyncUserType.SyncUserToDevice.ToString())
            {
                Grid_DevcieUsersSearchBox.Visibility = Visibility.Hidden;
                Grid_DeviceUsers.Visibility = Visibility.Hidden;
                ListView_DevcieBox.Visibility = Visibility.Visible;
                Grid_SystemUsers.Visibility = Visibility.Visible;
                tb_conditions.Text = "选择设备";
            }
            else
            {
                ListView_DevcieBox.Visibility = Visibility.Hidden;
                Grid_SystemUsers.Visibility = Visibility.Hidden;
                Grid_DevcieUsersSearchBox.Visibility = Visibility.Visible;
                Grid_DeviceUsers.Visibility = Visibility.Visible;
                tb_conditions.Text = "查询条件";
            }
        }
    }
}
