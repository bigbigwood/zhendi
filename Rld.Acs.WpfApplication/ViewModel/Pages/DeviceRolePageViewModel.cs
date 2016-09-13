using System.Collections;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Command;
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

        public List<DeviceRole> DeviceRoles
        {
            get { return ApplicationManager.GetInstance().AuthorizationDeviceRoles; }
        }
        public ObservableCollection<DeviceRoleViewModel> DeviceRoleViewModels { get; set; }
        public DeviceRoleViewModel SelectedDeviceRoleViewModel { get; set; }

        public DeviceRolePageViewModel()
        {
            AddCmd = new AuthCommand(AddDeviceController);
            ModifyCmd = new AuthCommand(ModifyDeviceController);
            DeleteCmd = new AuthCommand(DeleteDeviceController);

            var viewmodels = DeviceRoles.Select(x => new DeviceRoleViewModel(x));
            DeviceRoleViewModels = new ObservableCollection<DeviceRoleViewModel>(viewmodels);
        }

        private void AddDeviceController()
        {
            try
            {
                var viewModel = new DeviceRoleViewModel(new DeviceRole());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenDeviceRoleView);

                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    DeviceRoleViewModels.Add(viewModel);
                }
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

                var viewModel = new DeviceRoleViewModel(SelectedDeviceRoleViewModel.CurrentDeviceRole);
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenDeviceRoleView);

                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var index = DeviceRoleViewModels.IndexOf(SelectedDeviceRoleViewModel);
                    DeviceRoleViewModels[index] = new DeviceRoleViewModel(viewModel.ViewModelAttachment.CoreModel);
                }
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
