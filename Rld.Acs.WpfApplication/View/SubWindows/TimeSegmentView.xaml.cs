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
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View;

namespace Rld.Acs.WpfApplication.Views
{
    /// <summary>
    /// Interaction logic for TimeSegmentView.xaml
    /// </summary>
    public partial class TimeSegmentView : BaseWindow
    {
        public TimeSegmentView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.CloseTimeSegmentView, new Action<NotificationMessage>(ProcessCloseViewMessage));
        }
    }
}
