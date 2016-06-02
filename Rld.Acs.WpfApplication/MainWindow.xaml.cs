using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.CustomerControl;
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
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
