using System;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
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
