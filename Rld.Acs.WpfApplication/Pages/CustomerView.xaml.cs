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
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView
    {
        public CustomerView()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(this, Tokens.CloseCustomerView, new Action<NotificationMessage>((msg) => { Close(); }));
        }
    }
}
