using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View.Windows;

namespace Rld.Acs.WpfApplication.View.Pages
{
    /// <summary>
    /// Interaction logic for TimeSegmentPage.xaml
    /// </summary>
    public partial class SysOperatorPage : BasePage
    {
        public SysOperatorPage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.SysOperatorView_Open, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.SysOperatorPage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
            Messenger.Default.Register(this, Tokens.SysOperatorPage_ShowQuestion, new Action<NotificationMessageAction>((msg) => ShowQuestionAndAction(msg, "删除用户")));
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            var view = new SysOperatorView() { DataContext = msg.DataContext };
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();
        }
    }
}
