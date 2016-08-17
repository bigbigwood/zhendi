using System.Collections;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DevicePageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceControllerRepository _deviceControllerRepo  = NinjectBinder.GetRepository<IDeviceControllerRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public List<DeviceController> DeviceControllers { get; set; }
        public ObservableCollection<DeviceViewModel> DeviceControllerViewModels { get; set; }
        public DeviceViewModel SelectedDeviceViewModel { get; set; }

        public DevicePageViewModel()
        {
            AddCmd = new RelayCommand(AddDeviceController);
            ModifyCmd = new RelayCommand(ModifyDeviceController);
            DeleteCmd = new RelayCommand(DeleteDeviceController);

            DeviceControllerViewModels = new ObservableCollection<DeviceViewModel>();
            //DeviceControllers = _deviceControllerRepo.Query(new Hashtable()).ToList();
            DeviceControllers = ApplicationManager.GetInstance().AuthorizationDevices;
            DeviceControllers.ForEach(t => DeviceControllerViewModels.Add(new DeviceViewModel(t)));
        }

        private void AddDeviceController()
        {
            try
            {
                var deviceViewModel = new DeviceViewModel(new DeviceController());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = deviceViewModel

                }, Tokens.OpenDeviceView);

                if (deviceViewModel.CurrentDeviceController.DeviceID!= 0)
                    DeviceControllerViewModels.Add(deviceViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyDeviceController()
        {
            try
            {
                if (SelectedDeviceViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择设备!"), Tokens.DevicePage_ShowNotification);
                    return;
                }

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedDeviceViewModel

                }, Tokens.OpenDeviceView);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteDeviceController()
        {
            try
            {
                if (SelectedDeviceViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择设备!"), Tokens.TimeSegmentPage_ShowNotification);
                    return;
                }

                //if (AuthorizationDepartments.Any(d => d.Parent != null && d.Parent.DepartmentID == SelectedDepartmentDetailViewModel.CurrentDepartment.DepartmentID))
                //{
                //    Messenger.Default.Send(new NotificationMessage("选中部门存在子部门，请先删除所属子部门!"), Tokens.DepartmentPage_ShowNotification);
                //    return;
                //}

                string question = string.Format("确定删除设备:{0}吗？", SelectedDeviceViewModel.Name);
                Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmDeviceController), Tokens.DevicePage_ShowQuestion);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ConfirmDeviceController()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _deviceControllerRepo.Delete(SelectedDeviceViewModel.CurrentDeviceController.DeviceID);
                    message = "删除设备成功!";

                    DeviceControllerViewModels.Remove(SelectedDeviceViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除设备失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DevicePage_ShowQuestion);
            });
        }
    }
}
