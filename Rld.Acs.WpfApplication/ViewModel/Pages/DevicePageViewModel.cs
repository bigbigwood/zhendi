using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.ViewModel.Converter;
using Rld.Acs.WpfApplication.ViewModel.Views;
using Rld.Acs.WpfApplication.DeviceProxy;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class DevicePageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceControllerRepository _deviceControllerRepo  = NinjectBinder.GetRepository<IDeviceControllerRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public RelayCommand SearchNewDeviceCmd { get; private set; }
        public RelayCommand DeviceGroupMgntCmd { get; private set; }

        public List<DeviceController> DeviceControllers
        {
            get { return ApplicationManager.GetInstance().AuthorizationDevices; }
        }
        public ObservableCollection<DeviceViewModel> DeviceControllerViewModels { get; set; }
        public DeviceViewModel SelectedDeviceViewModel { get; set; }

        public DevicePageViewModel()
        {
            AddCmd = new AuthCommand(AddDeviceController);
            ModifyCmd = new AuthCommand(ModifyDeviceController);
            DeleteCmd = new AuthCommand(DeleteDeviceController);
            SearchNewDeviceCmd = new AuthCommand(SearchNewDevices);
            DeviceGroupMgntCmd = new RelayCommand(DeviceGroupMgnt);

            var deviceViewModels = DeviceControllers.Select(x => x.ToViewModel());
            DeviceControllerViewModels = new ObservableCollection<DeviceViewModel>(deviceViewModels);
        }

        private void AddDeviceController()
        {
            try
            {
                var viewModel = new DeviceController().ToViewModel();
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenDeviceView);

                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    DeviceControllerViewModels.Add(viewModel);
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
                if (SelectedDeviceViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.DevicePage_ShowNotification);
                    return;
                }

                var coreModel = SelectedDeviceViewModel.ToCoreModel();
                var viewModel = coreModel.ToViewModel();
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenDeviceView);

                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var index = DeviceControllerViewModels.IndexOf(SelectedDeviceViewModel);
                    DeviceControllerViewModels[index] = viewModel;
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
                if (SelectedDeviceViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.DevicePage_ShowNotification);
                    return;
                }

                string assiciationErrorMessage = "";
                var departmentAssiciationDeviceIds = ApplicationManager.GetInstance().AuthorizationDepartments
                    .SelectMany(x => x.DeviceAssociations).Select(x => x.DeviceID);
                if (departmentAssiciationDeviceIds.Contains(SelectedDeviceViewModel.Id))
                {
                    assiciationErrorMessage += LanguageManager.GetLocalizationResource(Resource.MSG_CannotDeleteDeviceBecauseOfDeptAssociation);
                }

                var deviceRoleAssiciationDeviceIds = ApplicationManager.GetInstance().AuthorizationDeviceRoles
                    .SelectMany(x => x.DeviceRolePermissions)
                    .Select(x => x.DeviceID);
                if (deviceRoleAssiciationDeviceIds.Contains(SelectedDeviceViewModel.Id))
                {
                    assiciationErrorMessage += LanguageManager.GetLocalizationResource(Resource.MSG_CannotDeleteDeviceBecauseOfRoleAssociation);
                }

                if (!string.IsNullOrWhiteSpace(assiciationErrorMessage))
                {
                    Messenger.Default.Send(new NotificationMessage(assiciationErrorMessage), Tokens.DevicePage_ShowNotification);
                    return;
                }

                string question = string.Format(LanguageManager.GetLocalizationResource(Resource.MSG_DoUWantToDeleteObject), SelectedDeviceViewModel.Name);
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
                    _deviceControllerRepo.Delete(SelectedDeviceViewModel.Id);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteSuccessfully);

                    DeviceControllerViewModels.Remove(SelectedDeviceViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteFail);
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DevicePage_ShowNotification);
            });
        }

        private void SearchNewDevices()
        {
            try
            {
                string[] messages;
                ResultTypes resultTypes;
                var deviceCodes = new DeviceServiceClient().SearchNewDevices(out resultTypes, out messages);
                if (resultTypes == ResultTypes.Ok)
                {
                    if (deviceCodes != null && deviceCodes.Any())
                    {
                        var deviceCodeString = string.Join(", ", deviceCodes);
                        string question = string.Format(LanguageManager.GetLocalizationResource(Resource.MSG_DoUWantToImportNewDevice), deviceCodeString);
                        Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmImportNewController), Tokens.DevicePage_ShowQuestion);
                    }
                    else
                    {
                        var message = LanguageManager.GetLocalizationResource(Resource.MSG_NoNewDeviceDetected);
                        Messenger.Default.Send(new NotificationMessage(message), Tokens.DevicePage_ShowNotification);
                    }
                }
                else
                {
                    var message = LanguageManager.GetLocalizationResource(Resource.MSG_SearchNewDeviceFail);
                    Messenger.Default.Send(new NotificationMessage(message), Tokens.DevicePage_ShowNotification);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ConfirmImportNewController()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    string[] messages;
                    ResultTypes resultTypes;
                    var newDevices = new DeviceServiceClient().SyncDevices(out resultTypes, out messages);

                    if (resultTypes == ResultTypes.Ok && newDevices != null && newDevices.Any())
                    {
                        newDevices.ForEach(x => DeviceControllerViewModels.Add(x.ToViewModel()));

                        var cacheableRepo = _deviceControllerRepo as CacheableRepository<DeviceController, int>;
                        cacheableRepo.Refresh();
                    }

                    message = LanguageManager.GetLocalizationResource(Resource.MSG_ImportNewDeviceSuccess);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_ImportNewDeviceFail);
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DevicePage_ShowNotification);
            });
        }

        private void DeviceGroupMgnt()
        {
            try
            {
                Messenger.Default.Send(new OpenWindowMessage(), Tokens.DeviceGroupPage_Open);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
