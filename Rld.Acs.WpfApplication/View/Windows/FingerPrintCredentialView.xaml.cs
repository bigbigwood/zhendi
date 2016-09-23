using System;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class FingerPrintCredentialView : BaseWindow
    {
        public FingerPrintCredentialView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.UserAuthenticationView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg文件(*.jpg)|*.jpg|png文件(*.png)|*.png|所有文件(*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] fingerprint = new byte[ImageFpDataLength];
                ConvertImageFileToFpBytes(openFileDialog.FileName, ref fingerprint);
                FpTextBox.Text = ConvertByteToHex(fingerprint).Replace("-", " ");

                var viewModel = DataContext as UserAuthenticationViewModel;
                viewModel.AuthenticationData = FpTextBox.Text;
            }
        }

        /// <summary>
        /// 逐字节变为16进制字符，以-隔开
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>16进制字符</returns>
        public static string ConvertByteToHex(byte[] bytes)
        {
            //string result = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append("-" + Convert.ToString(bytes[i], 16).PadLeft(2, '0'));
            }
            return sb.ToString().Remove(0, 1).ToUpper();
        }

        private const int ImageFpDataLength = 64372;
        private const int ImageFpWidth = 242;
        private const int ImageFpHeight = 266;
        public static void ConvertImageFileToFpBytes(string filename, ref byte[] data)
        {
            Image img = Image.FromFile(filename);

            Bitmap bmp = new Bitmap(ImageFpWidth, ImageFpHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int h = bmp.Height;
            int w = bmp.Width;
            int x = (ImageFpWidth - img.Width) / 2;
            int y = (ImageFpHeight - img.Height) / 2;
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, w, h);
            g.DrawImage(img, x, y, img.Width, img.Height);

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Color c = bmp.GetPixel(j, i);

                    data[i * w + j] = (byte)((float)(c.R + c.G + c.B) / 3.0f);
                }
            }
        }
    }
}
