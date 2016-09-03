using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View.Windows;
using Rld.Acs.WpfApplication.ViewModel.Pages;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.View.Pages
{
    /// <summary>
    /// Interaction logic for TimeSegmentPage.xaml
    /// </summary>
    public partial class FloorMonitorPage : BasePage
    {
        private FloorMonitorPageViewModel _viewModel;

        public FloorMonitorPage()
        {
            InitializeComponent();
        }

        private void FloorMonitorPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as FloorMonitorPageViewModel;
            foreach (var floorViewModel in _viewModel.FloorViewModels)
            {
                //PhotosListBox.ItemsSource
            }
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
        }

        private void OnStop(object sender, RoutedEventArgs e)
        {
        }

        private void OnPhotoClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainCanvas.Children.Clear();

            var listBox = sender as ListBox;
            var floorViewModel = listBox.SelectedItem as FloorViewModel;
            SetFloorPhoto(floorViewModel.Photo, MainCanvas);

            var floorDoors = floorViewModel.Doors.FindAll(x => x.FloorID == floorViewModel.FloorID);
            if (floorDoors.Any())
            {
                foreach (var floorDoor in floorDoors)
                {
                    var panel = CreateDoorControl(floorDoor, MainCanvas);
                    MainCanvas.Children.Add(panel);
                }
            }

            MainFloorName.Text = floorViewModel.Name;
        }

        private void DrawFloor(FloorViewModel floorViewModel)
        {
            
        }

        private void SetFloorPhoto(string imagePath, Canvas canvas)
        {
            var myBrush = new ImageBrush();
            var image = new Image()
            {
                Source = new BitmapImage(new Uri(imagePath)),
                Stretch = Stretch.Fill,
            };
            myBrush.ImageSource = image.Source;
            canvas.Background = myBrush;
        }
        private StackPanel CreateDoorControl(FloorDoorViewModel floorDoor, Canvas canvas)
        {
            var carBitmap = new BitmapImage(new Uri("pack://application:,,,/Images/device/door.png"));
            var carImg = new Image();
            carImg.Source = carBitmap;
            carImg.Width = 25;
            carImg.Height = 15;

            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(carImg);
            panel.Children.Add(new TextBlock() { Text = floorDoor.DoorName });
            panel.DataContext = floorDoor;

            var menu_open = new MenuItem() {Header = "开门"};
            var menu_close = new MenuItem() {Header = "关门"};
            var menu_keepOpen = new MenuItem() {Header = "长开门"};
            var menu_keepClose = new MenuItem() {Header = "长关门"};
            menu_open.Click += delegate(object sender, RoutedEventArgs args) { CloseDoor(); };
            menu_open.Click += delegate(object sender, RoutedEventArgs args) { CloseDoor(); };
            menu_open.Click += delegate(object sender, RoutedEventArgs args) { CloseDoor(); };
            menu_open.Click += delegate(object sender, RoutedEventArgs args) { CloseDoor(); };

            panel.ContextMenu = new ContextMenu();
            panel.ContextMenu.Items.Add(menu_open);
            panel.ContextMenu.Items.Add(menu_close);
            panel.ContextMenu.Items.Add(menu_keepOpen);
            panel.ContextMenu.Items.Add(menu_keepClose);

            Canvas.SetLeft(panel, floorDoor.LocationX * canvas.ActualWidth);
            Canvas.SetTop(panel, floorDoor.LocationY * canvas.ActualHeight);
            return panel;
        }

        private void CloseDoor()
        {
            
        }

        private void SmallCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            var canvas = sender as Canvas;
            var floorViewModel = canvas.DataContext as FloorViewModel;
            SetFloorPhoto(floorViewModel.Photo, canvas);
        }

        private void ResizeCanvas(Canvas canvas)
        {
            if (canvas.Children.Count == 0)
                return;

            var eles = new UIElement[canvas.Children.Count];
            canvas.Children.CopyTo(eles, 0);
            canvas.Children.Clear();

            foreach (var uiElement in eles)
            {
                var panel = uiElement as StackPanel;
                if (panel != null)
                {
                    var floorDoor = panel.DataContext as FloorDoorViewModel;
                    Canvas.SetLeft(panel, floorDoor.LocationX * canvas.ActualWidth);
                    Canvas.SetTop(panel, floorDoor.LocationY * canvas.ActualHeight);
                    canvas.Children.Add(panel);
                }
            }
        }

        private void FloorMonitorPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeCanvas(MainCanvas);
        }
    }
}
