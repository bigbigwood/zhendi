using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rld.Acs.Model;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.View.Windows;
using Rld.Acs.WpfApplication.ViewModel.Pages;
using Rld.Acs.WpfApplication.ViewModel.Views;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.View.Pages
{
    /// <summary>
    /// Interaction logic for TimeSegmentPage.xaml
    /// </summary>
    public partial class FloorMonitorPage : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FloorMonitorPageViewModel _viewModel;

        public FloorMonitorPage()
        {
            InitializeComponent();
        }

        private void FloorMonitorPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as FloorMonitorPageViewModel;
            if (_viewModel != null && _viewModel.FloorViewModels.Count > 1)
            {
                var firstFloorViewModel = _viewModel.FloorViewModels.FirstOrDefault();
                SetFloorCanvas(firstFloorViewModel);
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
            var listBox = sender as ListBox;
            var floorViewModel = listBox.SelectedItem as FloorViewModel;

            SetFloorCanvas(floorViewModel);
        }

        private void SetFloorCanvas(FloorViewModel floorViewModel)
        {
            MainCanvas.Children.Clear();
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


        private void SetFloorPhoto(string imagePath, Canvas canvas)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

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
            carImg.Height = 20;

            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            panel.Children.Add(carImg);
            panel.Children.Add(new TextBlock() { Text = floorDoor.DoorName, FontSize = 16});
            panel.DataContext = floorDoor;

            var menu_open = new MenuItem() { Header = "开门", Tag = panel};
            var menu_keepOpen = new MenuItem() { Header = "常开门", Tag = panel };
            var menu_keepClose = new MenuItem() { Header = "常关门", Tag = panel };
            var menu_autoControl = new MenuItem() { Header = "自动控制", Tag = panel };
            menu_open.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.Open);
            menu_keepOpen.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.KeepOpen);
            menu_keepClose.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.KeepClose);
            menu_autoControl.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.Auto);

            panel.ContextMenu = new ContextMenu();
            panel.ContextMenu.Items.Add(menu_open);
            panel.ContextMenu.Items.Add(menu_keepOpen);
            panel.ContextMenu.Items.Add(menu_keepClose);
            panel.ContextMenu.Items.Add(menu_autoControl);

            Canvas.SetLeft(panel, floorDoor.LocationX * canvas.ActualWidth);
            Canvas.SetTop(panel, floorDoor.LocationY * canvas.ActualHeight);
            return panel;
        }

        private async void UpdateDoorState(object sender, DoorControlOption option)
        {
            var menuItem = (MenuItem) sender;
            var panel = menuItem.Tag as StackPanel;
            var floordoor = panel.DataContext as FloorDoorViewModel;
            var doorInfo = ApplicationManager.GetInstance().AuthorizationDoors.FirstOrDefault(x => x.DeviceDoorID == floordoor.DoorID);

            string message = "";

            var controller = await MessageBoxSingleton.Instance.ShowProgressAsync("同步数据", "同步数据中，请稍等", false);
            controller.SetIndeterminate();

            await Task.Run(() =>
            {
                try
                {
                    Service.DeviceService.ResultTypes resultTypes;
                    bool resultTypeSpecified;
                    string[] messages;
                    Int32 deviceId = doorInfo.DeviceID;
                    Int32 doorIndex = doorInfo.DoorIndex;
                    var selectedOption = (Service.DeviceService.DoorControlOption)option.GetHashCode();

                    new Service.DeviceService.DeviceService().UpdateDoorState(deviceId, true, doorIndex, true,
                        selectedOption, true,
                        out resultTypes, out resultTypeSpecified, out messages);

                    message = "同步数据成功！";
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "同步数据失败！";
                }
            });

            await controller.CloseAsync();
            MessageBoxSingleton.Instance.ShowDialog(message, "");

            if (option == DoorControlOption.KeepClose)
            {
                UpdateDoorColerByState(panel, false);
            }
            else
            {
                UpdateDoorColerByState(panel, true);
            }
        }

        private void UpdateDoorColerByState(StackPanel panel, bool isOpen)
        {
            panel.Background = isOpen ? Brushes.Red : Brushes.Transparent;
        }

        private void SmallCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            var canvas = sender as Canvas;
            var floorViewModel = canvas.DataContext as FloorViewModel;
            SetFloorPhoto(floorViewModel.Photo, canvas);
        }

        private void FloorMonitorPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeCanvas(MainCanvas);
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
    }
}
