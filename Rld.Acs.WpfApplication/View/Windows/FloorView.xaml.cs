using System;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class FloorView : BaseWindow
    {
        public FloorView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.FloorView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.FloorView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }

        private void UploadPhothBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg文件(*.jpg)|*.jpg|png文件(*.png)|*.png|所有文件(*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                FloorPhotoPath.Text = openFileDialog.FileName;
                FloorPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
