using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class FloorView : BaseWindow
    {
        public FloorView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.FloorView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.FloorView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }

        private void UploadPhothBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg文件(*.jpg)|*.jpg|png文件(*.png)|*.png|所有文件(*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                FloorPhotoPath.Text = openFileDialog.FileName;

                ImageBrush myBrush = new ImageBrush();
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                image.Stretch = Stretch.Fill;

                myBrush.ImageSource = image.Source;

                MyCanvas.Background = myBrush;

                //FloorPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private ListBox dragSource = null;
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }
                    if (element == source)
                    {
                        return null;
                    }
                }
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            var parent = (Canvas)sender;
            object data = e.Data.GetData(typeof(FloorDoorViewModel));
            var floorDoor = data as FloorDoorViewModel;

            //((IList)dragSource.ItemsSource).Remove(data); // remove from source list box

            var carBitmap = new BitmapImage(new Uri("pack://application:,,,/Images/device/door.png"));
            var carImg = new Image();
            carImg.Source = carBitmap;
            carImg.Width = 25;
            carImg.Height = 15;

            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(carImg);
            panel.Children.Add(new TextBlock() {Text = floorDoor.DoorName});

            Canvas.SetLeft(panel, e.GetPosition(parent).X);
            Canvas.SetTop(panel, e.GetPosition(parent).Y);
            MyCanvas.Children.Add(panel);
        }
    }
}
