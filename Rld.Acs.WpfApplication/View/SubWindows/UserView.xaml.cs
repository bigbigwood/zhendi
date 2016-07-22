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
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View;

namespace Rld.Acs.WpfApplication.Views
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
