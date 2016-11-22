using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rld.Acs.WpfApplication.Service.Lisence;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            InitMessageBoxSingleton();
        }

        #region MessageBox Singleton

        private void InitMessageBoxSingleton()
        {
            MessageBoxSingleton.Instance.ShowDialog = ShowDialog;
            MessageBoxSingleton.Instance.ShowYesNo = ShowYesNo;
            MessageBoxSingleton.Instance.ShowProgressAsync = ShowProgressAsync2;
        }

        public async void ShowDialog(string message, string title)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "关闭",
                ColorScheme = MetroDialogColorScheme.Theme
            };
            MessageDialogResult result =
                await this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, mySettings);
        }

        public async void ShowYesNo(string message, string title, Action action)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "确定",
                NegativeButtonText = "取消",
                ColorScheme = MetroDialogColorScheme.Theme
            };
            MessageDialogResult result =
                await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
                await Task.Factory.StartNew(action);
        }

        public async Task<ProgressDialogController> ShowProgressAsync2(string title, string message, bool isCancelable = false)
        {
            return await this.ShowProgressAsync(title, message, isCancelable);
        }

        #endregion


        public void PopSubMenus(object sender, RoutedEventArgs e)
        {
            FullMenusPanelsContainer.Children.Clear();
            Button fireBtn = sender as Button;

            if (PopSubMenuPanel.Visibility == Visibility.Visible)
            {
                PopSubMenuPanelsContainer.Children.Clear();
            }

            if (fireBtn == organzationBtn)
            {
                PopSubMenuPanelsContainer.Children.Add(peopleMenu_panel);
            }
            else if (fireBtn == deviceBtn)
            {
                PopSubMenuPanelsContainer.Children.Add(devieMenu_panel);
            }
            else if (fireBtn == reportBtn)
            {
                PopSubMenuPanelsContainer.Children.Add(pubMenu_panel);
            }
            else if (fireBtn == systemBtn)
            {
                PopSubMenuPanelsContainer.Children.Add(syswhMenu_panel);
            }
            else if (fireBtn == backendBtn)
            {
                PopSubMenuPanelsContainer.Children.Add(supporMenu_panel);
            }

            if (PopSubMenuPanel.Visibility != Visibility.Visible)
            {
                PopSubMenuPanel.Visibility = Visibility.Visible;
            }
        }


        //逐项关闭子对象(通用) 
        private void hide_toolmuSecond_people(object sender, RoutedEventArgs e)
        {
            this.PopSubMenuPanelsContainer.Children.Clear();
            this.PopSubMenuPanel.Visibility = Visibility.Hidden;
        }





        //显示全选_右边框
        private void PopFullMenus(object sender, RoutedEventArgs e)
        {

            //清除二引用的对象,关闭二，三重新加载。
            this.PopSubMenuPanelsContainer.Children.Clear();
            this.PopSubMenuPanel.Visibility = Visibility.Hidden;
            //清除三重加载头部功能
            this.FullMenusPanelsContainer.Children.Clear();
            this.FullMenusPanelsContainer.Children.Add(toolmuright_top);

            //重加载对象
            this.FullMenusPanelsContainer.Children.Add(peopleMenu_panel);
            this.FullMenusPanelsContainer.Children.Add(devieMenu_panel);
            this.FullMenusPanelsContainer.Children.Add(pubMenu_panel);
            this.FullMenusPanelsContainer.Children.Add(syswhMenu_panel);
            this.FullMenusPanelsContainer.Children.Add(supporMenu_panel);
            this.FullMenusPanel.Visibility = Visibility.Visible;
        }




        //关闭全选_右边框
        public void toolmuright_close(object sender, RoutedEventArgs e)
        {
            //this.toolmuright.Visibility = Visibility.Hidden;
            this.FullMenusPanel.Visibility = Visibility.Hidden;
        }




        //控制仿树对象显示隐藏。
        public void tree_showHide_menu(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            //人员管理
            if (mi == peo_menu_right)
            {
                if (peo_control.Visibility == Visibility.Hidden)
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_open");
                    this.peo_control.Style = myStyle;
                }

                else
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_close");
                    this.peo_control.Style = myStyle;
                }

            }

            //设备管理
            if (mi == dev_menu_right)
            {
                if (dev_control.Visibility == Visibility.Hidden)
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_open");
                    this.dev_control.Style = myStyle;
                }

                else
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_close");
                    this.dev_control.Style = myStyle;
                }

            }

            //综合信息展示
            if (mi == pub_menu_right)
            {
                if (pub_control.Visibility == Visibility.Hidden)
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_open");
                    this.pub_control.Style = myStyle;
                }

                else
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_close");
                    this.pub_control.Style = myStyle;
                }

            }
            //系统维护
            if (mi == syswh_menu_right)
            {
                if (syswh_control.Visibility == Visibility.Hidden)
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_open");
                    this.syswh_control.Style = myStyle;
                }

                else
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_close");
                    this.syswh_control.Style = myStyle;
                }

            }

            //后台服务
            if (mi == suppor_menu_right)
            {
                if (suppor_control.Visibility == Visibility.Hidden)
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_open");
                    this.suppor_control.Style = myStyle;
                }

                else
                {
                    Style myStyle = (Style)this.FindResource("stack_tree_close");
                    this.suppor_control.Style = myStyle;
                }

            }
        }

        private void OperatorSettingBtn_OnClick(object sender, RoutedEventArgs e)
        {
            //popUserInfo.IsOpen = !popUserInfo.IsOpen;
        }

        private void LisenceWindowBtn_OnClick(object sender, RoutedEventArgs e)
        {
            //var lisence = LisenceService.GetLicense();
            //if (lisence != null)
            //{
            //    var lisenceWindow = new LisenceWindow();
            //    lisenceWindow.DataContext = lisence;
            //    lisenceWindow.ShowDialog();
            //    //if (lisenceWindow.Lisenced != true)
            //    //{
            //    //    Close();
            //    //}
            //}
        }
    }
}
