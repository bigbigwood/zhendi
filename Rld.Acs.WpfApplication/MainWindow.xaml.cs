using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rld.Acs.WpfApplication.CustomerControl;
using Rld.Acs.WpfApplication.Extension;
using Rld.Acs.WpfApplication.Pages;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rld.Acs.WpfApplication
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

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = (ToggleButton)sender;
            toggleButton.Expand = !toggleButton.Expand;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            UserMainWindow page = new UserMainWindow();
            MainFrame.Content = page;
        }

        private void InitMessageBoxSingleton()
        {
            MessageBoxSingleton.Instance.ShowDialog = ShowDialog;
            MessageBoxSingleton.Instance.ShowYesNo = ShowYesNo;
        }

        public async void ShowDialog(string message, string title)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "关闭",
                ColorScheme = MetroDialogColorScheme.Theme
            };
            MessageDialogResult result = await this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, mySettings);
        }

        public async void ShowYesNo(string message, string title, Action action)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "确定",
                NegativeButtonText = "取消",
                ColorScheme = MetroDialogColorScheme.Theme
            };
            MessageDialogResult result = await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
                await Task.Factory.StartNew(action);
        }
    }
}
