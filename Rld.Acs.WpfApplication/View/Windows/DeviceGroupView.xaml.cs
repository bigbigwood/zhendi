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
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for MoveUserView.xaml
    /// </summary>
    public partial class DeviceGroupView : BaseWindow
    {
        public DeviceGroupView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.DeviceGroupView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DeviceGroupView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }
    }
}
