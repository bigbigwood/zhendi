using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Service.Lisence;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for LisenceWindow.xaml
    /// </summary>
    public partial class LisenceWindow : BaseWindow
    {
        public LisenceWindow()
        {
            InitializeComponent();
        }

        public bool Lisenced = false;
        private void BtnActivate_OnClick(object sender, RoutedEventArgs e)
        {
            string key = tbKey.Text;

            if (LisenceService.UpdateLisence(key))
            {
                tbInfo.Text = "更新许可证成功，请关闭此窗口。";
                Lisenced = true;
            }

            else
                tbInfo.Text = "更新许可证失败，，请联系您的软件供应商。";
        }

        private void BtnTrial_OnClick(object sender, RoutedEventArgs e)
        {
            if (LisenceService.ApplyTrialLisence())
            {
                tbInfo.Text = "您可以试用14天，请在14天内联系您的软件供应商获取新的许可证。";
                Lisenced = true;
            }
            else
                tbInfo.Text = "试用失败，请联系您的软件供应商。";
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LisenceWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            tbSN.Text = LisenceTool.ToProductCodeFormat(SnProvider.CalculateSN());
            var lisence = DataContext as SimpleLicense;
            if (lisence != null && lisence.IsExpired)
            {
                tbInfo.Text = "您的许可证已经到期，请联系您的软件供应商获取新的许可证。";
                btnTrial.IsEnabled = false;
            }
        }
    }
}
