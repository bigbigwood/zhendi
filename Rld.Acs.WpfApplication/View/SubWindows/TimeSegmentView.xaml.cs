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
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Extension;
using Rld.Acs.WpfApplication.Messages;

namespace Rld.Acs.WpfApplication.Views
{
    /// <summary>
    /// Interaction logic for TimeSegmentView.xaml
    /// </summary>
    public partial class TimeSegmentView
    {
        public TimeSegmentView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseTimeSegmentView, new Action<NotificationMessage>(ProcessCloseViewMessage));
        }

        private void MetroWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }

        private void ProcessCloseViewMessage(NotificationMessage msg)
        {
            if (!string.IsNullOrWhiteSpace(msg.Notification))
            {
                MessageBoxSingleton.Instance.ShowDialog(msg.Notification, "");
            }

            Close();
        }
    }
}
