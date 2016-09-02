using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AutoMapper;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Microsoft.Win32;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class FloorView : BaseWindow
    {
        private FloorViewModel _floorViewModel;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserAvatorService _userAvatorService = new UserAvatorService();

        public FloorView()
        {
            InitializeComponent();

            Messenger.Default.Register(this, Tokens.FloorView_Close, new Action<NotificationMessage>(ProcessCloseViewMessage));
            Messenger.Default.Register(this, Tokens.FloorView_ShowNotification, new Action<NotificationMessage>(ShowSubViewNotification));
        }

        private void FloorView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _floorViewModel = DataContext as FloorViewModel;
            if (_floorViewModel == null)
            {
                throw new Exception("floorViewModel is not loaded in FloorView");
            }

            if (_floorViewModel.FloorID != 0)
            {
                if (!string.IsNullOrWhiteSpace(_floorViewModel.Photo))
                {
                    var imagePath = _userAvatorService.GetAvator(_floorViewModel.Photo);
                    ImageBrush myBrush = new ImageBrush();
                    Image image = new Image()
                    {
                        Source = new BitmapImage(new Uri(imagePath)),
                        Stretch = Stretch.Fill,
                    };
                    myBrush.ImageSource = image.Source;
                    MyCanvas.Background = myBrush;
                }
            }
        }

        private void UploadPhothBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg文件(*.jpg)|*.jpg|png文件(*.png)|*.png|所有文件(*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;

                string extension = new FileInfo(filename).Extension;

                string uniqueFileName = string.Format(@"{0}_{1}{2}", Guid.NewGuid(), DateTime.Now.ToString("yyyyMMddhhmmss"), extension);
                string cacheFilePath = string.Format(@"{0}\{1}", ApplicationManager.GetInstance().LocalImageCachePath, uniqueFileName);
                File.Copy(filename, cacheFilePath);

                ImageBrush myBrush = new ImageBrush();
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri(cacheFilePath)),
                    Stretch = Stretch.Fill,
                };
                myBrush.ImageSource = image.Source;
                MyCanvas.Background = myBrush;

                _floorViewModel.Photo = uniqueFileName;
            }
        }

        private void OkBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                _userAvatorService.UploadAvatorToServer(_floorViewModel.Photo);

                var coreModel = Mapper.Map<Floor>(_floorViewModel);
                MessageBoxSingleton.Instance.ShowDialog("保存成功", "");
                Close();
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        private void CancelBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private ListBox dragSource = null;
        private List<StackPanel> dropDoors = new List<StackPanel>();
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

        private void ListBox_OnDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(FloorDoorViewModel));
            var floorDoor = data as FloorDoorViewModel;

            var uiElement = dropDoors.FirstOrDefault(x => x.DataContext == floorDoor);
            if (uiElement != null)
            {
                MyCanvas.Children.Remove(uiElement);
                dropDoors.Remove(uiElement);
            }
        }

        private void Door_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var stackpanel = (StackPanel)sender;
            object data = stackpanel.DataContext;

            if (data != null)
            {
                DragDrop.DoDragDrop(MyCanvas, data, DragDropEffects.Move);
            }
        }

        private void myCanvas_OnDrop(object sender, DragEventArgs e)
        {
            var parent = (Canvas)sender;
            object data = e.Data.GetData(typeof(FloorDoorViewModel));
            var floorDoor = data as FloorDoorViewModel;

            var uiElement = dropDoors.FirstOrDefault(x => x.DataContext == floorDoor);
            if (uiElement != null)
            {
                MyCanvas.Children.Remove(uiElement);
                dropDoors.Remove(uiElement);
            }

            floorDoor.FloorID = _floorViewModel.FloorID;
            floorDoor.LocationX = e.GetPosition(parent).X;
            floorDoor.LocationY = e.GetPosition(parent).Y;

            var carBitmap = new BitmapImage(new Uri("pack://application:,,,/Images/device/door.png"));
            var carImg = new Image();
            carImg.Source = carBitmap;
            carImg.Width = 25;
            carImg.Height = 15;

            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(carImg);
            panel.Children.Add(new TextBlock() { Text = floorDoor.DoorName });
            panel.DataContext = floorDoor;
            panel.PreviewMouseLeftButtonDown += Door_PreviewMouseLeftButtonDown;

            Canvas.SetLeft(panel, floorDoor.LocationX);
            Canvas.SetTop(panel, floorDoor.LocationY);
            MyCanvas.Children.Add(panel);
            Point currentPonit = e.GetPosition(parent);
            Point newpoint = new Point(currentPonit.X / MyCanvas.ActualWidth, currentPonit.Y/MyCanvas.ActualHeight);

            panel.Tag = newpoint;
            dropDoors.Add(panel);
        }

        private void MyCanvas_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MyCanvas.Children.Count == 0)
                return;


            var eles = new UIElement[MyCanvas.Children.Count];
            MyCanvas.Children.CopyTo(eles, 0);
            MyCanvas.Children.Clear();

            foreach (var uiElement in eles)
            {
                var panel = uiElement as StackPanel;
                if (panel != null)
                {
                    Point location = (Point)panel.Tag;
                    double x = location.X* MyCanvas.ActualWidth;
                    double y = location.Y* MyCanvas.ActualHeight;

                    Canvas.SetLeft(panel, x);
                    Canvas.SetTop(panel, y);
                    MyCanvas.Children.Add(panel);
                }
            }
        }

    }
}
