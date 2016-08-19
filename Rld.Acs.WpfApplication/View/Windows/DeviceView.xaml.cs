using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.WpfApplication.Models.Messages;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class DeviceView : BaseWindow
    {
        public DeviceView()
        {
            InitializeComponent();

            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenDeviceDoorView, ProcessOpenView);
            Messenger.Default.Register<OpenWindowMessage>(this, Tokens.OpenDeviceHeadReadingView, ProcessOpenView);
            Messenger.Default.Register(this, Tokens.CloseDeviceView, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.DeviceView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }

        private void OnNextPageClicked(object sender, RoutedEventArgs e)
        {
            myTab.SelectedIndex++;
        }

        private void ProcessOpenView(OpenWindowMessage msg)
        {
            if (msg.WindowType == "DeviceDoorView")
            {
                var view = new DeviceDoorView() { DataContext = msg.DataContext };
                view.BorderThickness = new Thickness(1);
                view.GlowBrush = null;
                view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
                view.ShowDialog(); 
            }
            else if (msg.WindowType == "DeviceHeadReadingView")
            {
                var view = new DeviceHeadReadingView() { DataContext = msg.DataContext };
                view.BorderThickness = new Thickness(1);
                view.GlowBrush = null;
                view.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
                view.ShowDialog();
            }

        }
    }
}
