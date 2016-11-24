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
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.Service.Validator;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.View.Windows
{
    /// <summary>
    /// Interaction logic for TimeGroupView.xaml
    /// </summary>
    public partial class FloorView : BaseWindow
    {
        private FloorViewModel _floorViewModel;
        private bool _photoChanged = false;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserAvatorService _userAvatorService = new UserAvatorService();
        private IFloorRepository _floorRepo = NinjectBinder.GetRepository<IFloorRepository>();
        private ListBox dragSource = null;
        private List<StackPanel> dropDoors = new List<StackPanel>();

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
                    SetFloorPhoto(imagePath);
                }

                var floorDoors = _floorViewModel.Doors.FindAll(x => x.FloorID == _floorViewModel.FloorID);
                if (floorDoors.Any())
                {
                    foreach (var floorDoor in floorDoors)
                    {
                        var panel = CreateDoorControl(floorDoor);
                        MyCanvas.Children.Add(panel);
                        dropDoors.Add(panel);
                    }
                }
            }
        }

        private void UploadPhothBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = LanguageManager.GetLocalizationResource(Resource.MSG_FloorImageFilter);
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                var filename = openFileDialog.FileName;
                var fileInfo = new FileInfo(filename);
                if (fileInfo.Length > 4*1024*1024)
                {
                    ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_FloorImageMaxSize)));
                    return;
                }

                string extension = new FileInfo(filename).Extension;
                string uniqueFileName = string.Format(@"{0}_{1}{2}", Guid.NewGuid(), DateTime.Now.ToString("yyyyMMddhhmmss"), extension);
                string cacheFilePath = string.Format(@"{0}\{1}", ApplicationEnvironment.LocalImageCachePath, uniqueFileName);
                File.Copy(filename, cacheFilePath);

                SetFloorPhoto(cacheFilePath);
                _floorViewModel.Photo = uniqueFileName;
                _photoChanged = true;
            }
        }

        private void OkBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (_photoChanged)
                {
                    _userAvatorService.UploadAvatorToServer(_floorViewModel.Photo);
                }

                var coreModel = Mapper.Map<Floor>(_floorViewModel);
                foreach (var dropDoor in dropDoors)
                {
                    var floorDoorViewModel = Mapper.Map<FloorDoor>(dropDoor.DataContext as FloorDoorViewModel);
                    coreModel.Doors.Add(floorDoorViewModel);
                }

                var validator = NinjectBinder.GetValidator<FloorValidator>();
                var results = validator.Validate(coreModel);
                if (!results.IsValid)
                {
                    var message = string.Join("\n", results.Errors);
                    ShowSubViewNotification(new NotificationMessage(message));
                    return;
                }

                if (coreModel.FloorID == 0)
                {
					coreModel.Status = GeneralStatus.Enabled;
                    coreModel = _floorRepo.Insert(coreModel);
                    _floorViewModel.FloorID = coreModel.FloorID;
                    _floorViewModel.BindDoors(coreModel.Doors);
                    //UpdateAuthorizationDoorsForFloor(coreModel);
                }
                else
                {
                    _floorRepo.Update(coreModel);
                    coreModel = _floorRepo.GetByKey(coreModel.FloorID);
                    _floorViewModel.BindDoors(coreModel.Doors);
                    //UpdateAuthorizationDoorsForFloor(coreModel);
                }

                _floorViewModel.ViewModelAttachment.CoreModel = coreModel;
                _floorViewModel.ViewModelAttachment.LastOperationSuccess = true;
                ProcessCloseViewMessage(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SaveSuccessfully)));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowSubViewNotification(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SaveFail)));
            }
        }

        private void CancelBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }


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
            floorDoor.LocationX = e.GetPosition(parent).X / MyCanvas.ActualWidth;
            floorDoor.LocationY = e.GetPosition(parent).Y / MyCanvas.ActualHeight;

            var panel = CreateDoorControl(floorDoor);
            MyCanvas.Children.Add(panel);
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
                    var floorDoor = panel.DataContext as FloorDoorViewModel;
                    Canvas.SetLeft(panel, floorDoor.LocationX * MyCanvas.ActualWidth);
                    Canvas.SetTop(panel, floorDoor.LocationY * MyCanvas.ActualHeight);
                    MyCanvas.Children.Add(panel);
                }
            }
        }

        private void SetFloorPhoto(string imagePath)
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
            MyCanvas.Background = myBrush;
        }

        private StackPanel CreateDoorControl(FloorDoorViewModel floorDoor)
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
            panel.PreviewMouseLeftButtonDown += Door_PreviewMouseLeftButtonDown;
            Canvas.SetLeft(panel, floorDoor.LocationX * MyCanvas.ActualWidth);
            Canvas.SetTop(panel, floorDoor.LocationY * MyCanvas.ActualHeight);
            return panel;
        }

    }
}
