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
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
