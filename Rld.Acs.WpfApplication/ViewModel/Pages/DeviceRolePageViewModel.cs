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
    public class DeviceRolePageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceRoleRepository _deviceRoleRepo = NinjectBinder.GetRepository<IDeviceRoleRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public List<DeviceRole> DeviceRoles { get; set; }
        public ObservableCollection<DeviceRoleViewModel> DeviceRoleViewModels { get; set; }
        public DeviceRoleViewModel SelectedDeviceRoleViewModel { get; set; }

        public DeviceRolePageViewModel()
        {
            AddCmd = new RelayCommand(AddDeviceController);
            ModifyCmd = new RelayCommand(ModifyDeviceController);
            DeleteCmd = new RelayCommand(DeleteDeviceController);

            DeviceRoleViewModels = new ObservableCollection<DeviceRoleViewModel>();
            DeviceRoles = ApplicationManager.GetInstance().AuthorizationDeviceRoles;
            DeviceRoles.ForEach(t => DeviceRoleViewModels.Add(new DeviceRoleViewModel(t)));
        }

        private void AddDeviceController()
        {
            try
            {
                var deviceRoleViewModel = new DeviceRoleViewModel(new DeviceRole());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = deviceRoleViewModel

                }, Tokens.OpenDeviceRoleView);

                if (deviceRoleViewModel.CurrentDeviceRole.DeviceRoleID != 0)
                    DeviceRoleViewModels.Add(deviceRoleViewModel);
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
                if (SelectedDeviceRoleViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择设备角色!"), Tokens.DeviceRolePage_ShowNotification);
                    return;
                }

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedDeviceRoleViewModel

                }, Tokens.OpenDeviceRoleView);

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
                if (SelectedDeviceRoleViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择设备角色!"), Tokens.DeviceRolePage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除设备角色:{0}吗？", SelectedDeviceRoleViewModel.Name);
                Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmDeviceRole), Tokens.DeviceRolePage_ShowQuestion);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ConfirmDeviceRole()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _deviceRoleRepo.Delete(SelectedDeviceRoleViewModel.CurrentDeviceRole.DeviceRoleID);
                    message = "删除设备角色成功!";

                    DeviceRoleViewModels.Remove(SelectedDeviceRoleViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除设备角色失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceRolePage_ShowNotification);
            });
        }
    }
}
