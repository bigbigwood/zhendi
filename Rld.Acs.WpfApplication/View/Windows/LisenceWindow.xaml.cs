﻿using System;
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
            if (string.IsNullOrWhiteSpace(key))
            {
                tbInfo.Text = "请输入有效许可证。";
                return;
            }
             var lisence = DataContext as SimpleLicense;
            if (lisence != null && LisenceTool.ToProductCodeFormat(lisence.LicenseKey) == key)
            {
                tbInfo.Text = "此许可证已经被使用了, 请更换新的许可证。";
                return;
            }


            if (LisenceService.UpdateLisence(key))
            {
                tbInfo.Text = "更新许可证成功，请关闭此窗口。";
                Lisenced = true;
            }

            else
                tbInfo.Text = "更新许可证失败， 请联系您的软件供应商。";
        }

        private void BtnTrial_OnClick(object sender, RoutedEventArgs e)
        {
            if (LisenceService.ApplyTrialLisence())
            {
                tbInfo.Text = "您可以试用14天，请及时联系您的软件供应商获取新的许可证。";
                Lisenced = true;
                btnTrial.IsEnabled = false;
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
            if (lisence != null)
            {
                tbKey.Text = LisenceTool.ToProductCodeFormat(lisence.LicenseKey);
                if (lisence.IsExpired)
                {
                    tbInfo.Text = "您的许可证已经到期，请联系您的软件供应商获取新的许可证。";
                    btnTrial.IsEnabled = false;
                }

                int days = (lisence.ExpireDateTime - DateTime.Now).Days + 1;
                tbInfo.Text = string.Format("您的许可证还有{0}天到期，请及时联系您的软件供应商获取新的许可证。", days);
                btnTrial.IsEnabled = false;
            }
        }
    }
}
