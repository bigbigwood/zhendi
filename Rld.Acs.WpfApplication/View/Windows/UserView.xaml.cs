using System;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : BaseWindow
    {
        public UserView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseUserView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.UserView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg文件(*.jpg)|*.jpg|png文件(*.png)|*.png|所有文件(*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                tb_AvatorfilePath.Text = openFileDialog.FileName;
                img_avator.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void OnNextPageClicked(object sender, RoutedEventArgs e)
        {
            myTab.SelectedIndex ++;
        }

        private void OnPreviousPageClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
