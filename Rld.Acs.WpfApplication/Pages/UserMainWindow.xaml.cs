using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Messages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rld.Acs.WpfApplication.Pages
{
    /// <summary>
    /// UserMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserMainWindow : Page
    {
        public UserMainWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenCustomerView, (msg) => OpenCustomerView(msg));
        }

        private void OpenCustomerView(OpenWindowMessage msg)
        {
            var customerView = new CustomerView();
            customerView.DataContext = msg.DataContext;
            customerView.ShowDialog();
        }
    }
}
