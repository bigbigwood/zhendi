using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.DeviceProxy;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.View.Windows;
using Rld.Acs.WpfApplication.ViewModel.Pages;
using Rld.Acs.WpfApplication.ViewModel.Views;
using System.Threading.Tasks;
using DoorControlOption = Rld.Acs.Model.DoorControlOption;

namespace Rld.Acs.WpfApplication.View.Pages
{
    /// <summary>
    /// Interaction logic for TimeSegmentPage.xaml
    /// </summary>
    public partial class FloorMonitorPage : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FloorMonitorPageViewModel _viewModel;
        private FloorViewModel _selectedFloorViewModel;
        private System.Timers.Timer _timer;
        private const Int32 DefaultTimerIntervalMillSeconds = 10 * 1000;
        private Dictionary<Int32, StackPanel> _dropDoorDict = new Dictionary<Int32, StackPanel>();

        public FloorMonitorPage()
        {
            InitializeComponent();
        }

        private void FloorMonitorPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel = DataContext as FloorMonitorPageViewModel;
                if (_viewModel != null && _viewModel.FloorViewModels.Count > 1)
                {
                    var firstFloorViewModel = _viewModel.FloorViewModels.FirstOrDefault();
                    SelectFloorCanvas(firstFloorViewModel);
                }

                BtnStartMonitor.IsEnabled = true;
                BtnStopMonitor.IsEnabled = false;

                _timer = new System.Timers.Timer();
                _timer.Elapsed += OnTimedEvent;
                _timer.Interval = DefaultTimerIntervalMillSeconds;
                //_timer.Enabled = true;
            }
            catch (Exception ex)
            {
                Log.Error("OnTimedEvent error", ex);
                MessageBoxSingleton.Instance.ShowDialog("内部错误", "");
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                Log.Info("Trigger Floor monitor timer event...");
                if (_selectedFloorViewModel != null && _selectedFloorViewModel.Doors.Any())
                {
                    Log.InfoFormat("Floor monitor timer check doors for floor id: {0}", _selectedFloorViewModel.FloorID);
                    foreach (var floordoorId in _dropDoorDict.Keys)
                    {
                        var doorInfo = FloorDoorManager.GetInstance().AuthorizationDoors.FirstOrDefault(x => x.DeviceDoorID == floordoorId);
                        var deviceCode = ApplicationManager.GetInstance().AuthorizationDevices.First(x => x.DeviceID == doorInfo.DeviceID).Code.ToInt32();
                        ResultTypes resultTypes;
                        string[] messages;
                        Int32 doorIndex = doorInfo.DoorIndex;

                        bool isopened = new DeviceServiceClient().GetDoorState(deviceCode, doorIndex, out resultTypes, out messages);
                        if (resultTypes == ResultTypes.Ok)
                        {
                            Log.InfoFormat("Floor monitor timer gets state result: [doorId={0}, isopened={1}]", floordoorId, isopened);
                            Dispatcher.Invoke(() =>
                            {
                                UpdateDoorColerByState(_dropDoorDict[floordoorId], isopened);
                            });
                        }
                        else
                        {
                            Log.Warn(messages.ConvertToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnTimedEvent error", ex);
                MessageBoxSingleton.Instance.ShowDialog("内部错误", "");
            }
        }


        private void OnStart(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.InfoFormat("Floor monitor timer start witn interval second: {0}...", _viewModel.IntervalSeconds);
                _timer.Interval = _viewModel.IntervalSeconds * 1000;
                _timer.Start();

                BtnStartMonitor.IsEnabled = false;
                BtnStopMonitor.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Log.Error("OnPhotoClick error", ex);
                MessageBoxSingleton.Instance.ShowDialog("内部错误", "");
            }
        }

        private void OnStop(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Info("Floor monitor timer stop...");
                _timer.Stop();

                BtnStartMonitor.IsEnabled = true;
                BtnStopMonitor.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Log.Error("OnPhotoClick error", ex);
                MessageBoxSingleton.Instance.ShowDialog("内部错误", "");
            }
        }

        private void OnPhotoClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var listBox = sender as ListBox;
                var floorViewModel = listBox.SelectedItem as FloorViewModel;

                SelectFloorCanvas(floorViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("OnPhotoClick error", ex);
                MessageBoxSingleton.Instance.ShowDialog("内部错误", "");
            }
        }

        private void SmallCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var canvas = sender as Canvas;
                var floorViewModel = canvas.DataContext as FloorViewModel;
                SetFloorPhoto(floorViewModel.Photo, canvas);
            }
            catch (Exception ex)
            {
                Log.Error("SmallCanvas_Loaded error", ex);
                throw;
            }
        }

        private void FloorMonitorPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                ResizeCanvas(MainCanvas);
            }
            catch (Exception ex)
            {
                Log.Error("Resize error", ex);
                MessageBoxSingleton.Instance.ShowDialog("内部错误", "");
            }
        }

        private void SelectFloorCanvas(FloorViewModel floorViewModel)
        {
            MainCanvas.Children.Clear();
            _dropDoorDict.Clear();
            SetFloorPhoto(floorViewModel.Photo, MainCanvas);

            var floorDoors = floorViewModel.Doors.FindAll(x => x.FloorID == floorViewModel.FloorID);
            if (floorDoors.Any())
            {
                foreach (var floorDoor in floorDoors)
                {
                    var panel = CreateDoorControl(floorDoor, MainCanvas);
                    _dropDoorDict.Add(floorDoor.DoorID, panel);
                    MainCanvas.Children.Add(panel);
                }
            }

            MainFloorName.Text = floorViewModel.Name;
            _selectedFloorViewModel = floorViewModel;
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
            panel.Children.Add(new TextBlock() { Text = floorDoor.DoorName, FontSize = 16 });
            panel.DataContext = floorDoor;

            var menu_open = new MenuItem() { Header = "开门", Tag = panel };
            var menu_close = new MenuItem() { Header = "关门", Tag = panel };
            var menu_keepOpen = new MenuItem() { Header = "常开门", Tag = panel };
            var menu_keepClose = new MenuItem() { Header = "常关门", Tag = panel };
            var menu_autoControl = new MenuItem() { Header = "自动控制", Tag = panel };
            var menu_cancleAlarm = new MenuItem() { Header = "取消报警", Tag = panel };
            var menu_viewStuffList = new MenuItem() { Header = "门内人员列表", Tag = panel };
            menu_open.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.Open);
            menu_close.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.Close);
            menu_keepOpen.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.KeepOpen);
            menu_keepClose.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.KeepClose);
            menu_autoControl.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.Auto);
            menu_cancleAlarm.Click += (sender, args) => UpdateDoorState(sender, DoorControlOption.CancelAlarm);
            menu_viewStuffList.Click += (sender, args) => ViewStuff(sender);

            panel.ContextMenu = new ContextMenu();
            panel.ContextMenu.Items.Add(menu_open);
            panel.ContextMenu.Items.Add(menu_close);
            panel.ContextMenu.Items.Add(menu_keepOpen);
            panel.ContextMenu.Items.Add(menu_keepClose);
            panel.ContextMenu.Items.Add(menu_autoControl);
            panel.ContextMenu.Items.Add(menu_cancleAlarm);
            panel.ContextMenu.Items.Add(menu_viewStuffList);

            Canvas.SetLeft(panel, floorDoor.LocationX * canvas.ActualWidth);
            Canvas.SetTop(panel, floorDoor.LocationY * canvas.ActualHeight);
            return panel;
        }

        private async void ViewStuff(object sender)
        {
            var menuItem = (MenuItem)sender;
            var panel = menuItem.Tag as StackPanel;
            var floordoor = panel.DataContext as FloorDoorViewModel;
            var doorInfo = FloorDoorManager.GetInstance().AuthorizationDoors.FirstOrDefault(x => x.DeviceDoorID == floordoor.DoorID);
            var deviceInfo = ApplicationManager.GetInstance().AuthorizationDevices.First(x => x.DeviceID == doorInfo.DeviceID);

            var inHouseUserService = new InHouseUserService();
            var deviceGroupEnabled = inHouseUserService.HasBindDeviceGroup(deviceInfo);

            if (deviceGroupEnabled == false)
            {
                MessageBoxSingleton.Instance.ShowDialog("此设备还未绑定设备组，无法查看人员列表", "");
                return;
            }

            string errorMessage = "";
            List<User> inHouseUsers = null;

            var controller = await MessageBoxSingleton.Instance.ShowProgressAsync("准备数据", "准备数据中，请稍等", false);
            controller.SetIndeterminate();

            await Task.Run(() =>
            {
                try
                {
                    inHouseUsers = inHouseUserService.GetInHouseUsers(deviceInfo);


                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    errorMessage = "查看人员列表失败！";
                }
            });

            await controller.CloseAsync();

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                MessageBoxSingleton.Instance.ShowDialog(errorMessage, "");
                return;
            }

            if (inHouseUsers != null)
            {
                var vms = new List<UserViewModel>();
                inHouseUsers.ForEach(x => vms.Add(new UserViewModel(x)));

                var inHouseUserViewModel = new InHouseUserViewModel()
                {
                    UserViewModels = new ObservableCollection<UserViewModel>(vms)
                };

                var view = new InHouseUserView() { DataContext = inHouseUserViewModel };
                view.ShowDialog();
            }
        }

        private async void UpdateDoorState(object sender, DoorControlOption option)
        {
            var menuItem = (MenuItem)sender;
            var panel = menuItem.Tag as StackPanel;
            var floordoor = panel.DataContext as FloorDoorViewModel;
            var doorInfo = FloorDoorManager.GetInstance().AuthorizationDoors.FirstOrDefault(x => x.DeviceDoorID == floordoor.DoorID);
            var deviceCode = ApplicationManager.GetInstance().AuthorizationDevices.First(x => x.DeviceID == doorInfo.DeviceID).Code.ToInt32();

            string message = "";

            var controller = await MessageBoxSingleton.Instance.ShowProgressAsync("同步数据", "同步数据中，请稍等", false);
            controller.SetIndeterminate();

            await Task.Run(() =>
            {
                try
                {
                    string[] messages;

                    Int32 doorIndex = doorInfo.DoorIndex;
                    var selectedOption = (DeviceProxy.DoorControlOption)option.GetHashCode();

                    ResultTypes resultTypes = new DeviceServiceClient().UpdateDoorState(deviceCode, doorIndex, selectedOption, out messages);
                    message = MessageHandler.GenerateDeviceMessage(resultTypes, "操作设备成功！", "操作设备失败！");
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "操作设备失败！";
                }
            });

            await controller.CloseAsync();
            MessageBoxSingleton.Instance.ShowDialog(message, "");

            if (option == DoorControlOption.KeepClose)
            {
                UpdateDoorColerByState(panel, false);
            }
            else if (option == DoorControlOption.Open || option == DoorControlOption.KeepOpen)
            {
                UpdateDoorColerByState(panel, true);
            }
        }

        private void UpdateDoorColerByState(StackPanel panel, bool isOpen)
        {
            panel.Background = isOpen ? Brushes.Red : Brushes.Transparent;
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
