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
    /// Interaction logic for TimeGroupPage.xaml
    /// </summary>
    public partial class TimeGroupPage : BasePage
    {
        public TimeGroupPage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenTimeGroupView, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.TimeGroupPage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
            Messenger.Default.Register(this, Tokens.TimeGroupPage_ShowQuestion, new Action<NotificationMessageAction>((msg) => ShowQuestionAndAction(msg, "删除时间组")));
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            var view = new TimeGroupView() { DataContext = msg.DataContext };
            view.BorderThickness = new Thickness(1);
            view.GlowBrush = null;
            view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
            view.ShowDialog();
        }
    }
}
