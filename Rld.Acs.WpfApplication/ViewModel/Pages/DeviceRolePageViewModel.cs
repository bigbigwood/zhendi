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
using Rld.Acs.WpfApplication.Service.Language;
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
                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.OpenDeviceRoleView);
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
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.DeviceRolePage_ShowNotification);
                    return;
                }

                var viewModel = new DeviceRoleViewModel(SelectedDeviceRoleViewModel.CurrentDeviceRole);
                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.OpenDeviceRoleView);

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
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.DeviceRolePage_ShowNotification);
                    return;
                }

                string assiciationErrorMessage = "";
                var departmentAssiciationRoleIds = ApplicationManager.GetInstance().AuthorizationDepartments.Select(x => x.DeviceRoleID);
                if (departmentAssiciationRoleIds.Contains(SelectedDeviceRoleViewModel.CurrentDeviceRole.DeviceRoleID))
                {
                    assiciationErrorMessage += LanguageManager.GetLocalizationResource(Resource.MSG_CannotDeleteDeviceRoleBecauseOfDeptAssociation) + Environment.NewLine;
                }

                var userDeviceRoleRepo = NinjectBinder.GetRepository<IUserDeviceRoleRepository>();
                var userDeviceRoleAssiciation = userDeviceRoleRepo.Query(new Hashtable() { { "DeviceRoleID", SelectedDeviceRoleViewModel.CurrentDeviceRole.DeviceRoleID } });
                if (userDeviceRoleAssiciation.Any())
                {
                    assiciationErrorMessage += LanguageManager.GetLocalizationResource(Resource.MSG_CannotDeleteDeviceRoleBecauseOfStuffAssociation) + Environment.NewLine;
                }

                if (!string.IsNullOrWhiteSpace(assiciationErrorMessage))
                {
                    Messenger.Default.Send(new NotificationMessage(assiciationErrorMessage), Tokens.DeviceRolePage_ShowNotification);
                    return;
                }

                string question = string.Format(LanguageManager.GetLocalizationResource(Resource.MSG_DoUWantToDeleteObject), SelectedDeviceRoleViewModel.Name);
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
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteSuccessfully);

                    DeviceRoleViewModels.Remove(SelectedDeviceRoleViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteFail);
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceRolePage_ShowNotification);
            });
        }
    }
}
