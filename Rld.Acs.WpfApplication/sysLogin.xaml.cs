﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.ViewModel;

namespace Rld.Acs.WpfApplication.mainFrame
{
    /// <summary>
    /// Interaction logic for sysLogin.xaml
    /// </summary>
    public partial class sysLogin : MetroWindow
    {
        public sysLogin()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            
        }

        public void userLogin(object sender, RoutedEventArgs e)
        {
            string user = this.userName.Text.ToString();
            string pass = this.passWord.Text.ToString();
            if (user.Equals("admin") && pass.Equals("123456"))
            {
                Window mainWin = new MainWindow();
                Application.Current.MainWindow = mainWin;
                //mainWin.DataContext = ViewModelLocator
                mainWin.Show();
                this.Close();
            }
            else
            {
                this.loginInfo.Content = "登陆信息错误";
                this.loginInfo.Visibility = Visibility.Visible;
            }
        
        }

        public void loginRevoke(object sender, RoutedEventArgs e)
        {
            this.userName.Text = " ";
            this.passWord.Text = " ";
        }
        
    }
}
