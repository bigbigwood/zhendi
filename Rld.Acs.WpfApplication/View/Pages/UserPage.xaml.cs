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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : BasePage
    {
        public UserPage()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenUserView, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.UserPage_ShowNotification, new Action<NotificationMessage>(ShowMessage));
            Messenger.Default.Register(this, Tokens.UserPage_ShowQuestion, new Action<NotificationMessageAction>(msg => ShowQuestionAndAction(msg, "删除人员")));
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            BaseWindow view;
            if (msg.WindowType == "MoveUserView")
            {
                view = new MoveUserView { DataContext = msg.DataContext };
                view.BorderThickness = new Thickness(1);
                view.GlowBrush = null;
                view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
                view.ShowDialog();
            }
            else if (msg.WindowType == "SyncUserView")
            {
                view = new SyncUserView { DataContext = msg.DataContext };
                view.BorderThickness = new Thickness(1);
                view.GlowBrush = null;
                view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
                view.ShowDialog();
            }
            else
            {
                view = new UserView { DataContext = msg.DataContext };
                view.BorderThickness = new Thickness(1);
                view.GlowBrush = null;
                view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
                view.ShowDialog();
            }
        }


    }
}
