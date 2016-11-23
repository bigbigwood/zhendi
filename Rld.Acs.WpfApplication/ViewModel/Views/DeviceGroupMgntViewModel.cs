using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.ViewModel.Views;
using GalaSoft.MvvmLight.Threading;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class DeviceGroupMgntViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceGroupRepository _deviceGroupRepo = NinjectBinder.GetRepository<IDeviceGroupRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public RelayCommand QuitCmd { get; private set; }
        public ObservableCollection<DeviceGroupViewModel> DeviceGroupViewModels { get; set; }
        public DeviceGroupViewModel SelectedDeviceGroupViewModel { get; set; }


        public DeviceGroupMgntViewModel()
        {
            AddCmd = new RelayCommand(Add);
            ModifyCmd = new RelayCommand(Modify);
            DeleteCmd = new RelayCommand(ShowDeletionQuestion);
            QuitCmd = new RelayCommand(() => Close(""));

            var operators = _deviceGroupRepo.Query(new Hashtable());
            var vms = operators.Select(Mapper.Map<DeviceGroupViewModel>);
            DeviceGroupViewModels = new ObservableCollection<DeviceGroupViewModel>(vms);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.DeviceGroupPage_Close);
        }

        private void Add()
        {
            try
            {
                var viewModel = Mapper.Map<DeviceGroupViewModel>(new DeviceGroup());
                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.DeviceGroupView_Open);
                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    viewModel = Mapper.Map<DeviceGroupViewModel>(viewModel.ViewModelAttachment.CoreModel);
                    DeviceGroupViewModels.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Modify()
        {
            try
            {
                if (SelectedDeviceGroupViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.DeviceGroupPage_ShowNotification);
                    return;
                }

                var coreModel = Mapper.Map<DeviceGroup>(SelectedDeviceGroupViewModel);
                var viewModel = Mapper.Map<DeviceGroupViewModel>(coreModel);

                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.DeviceGroupView_Open);
                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var index = DeviceGroupViewModels.IndexOf(SelectedDeviceGroupViewModel);
                    DeviceGroupViewModels[index] = viewModel;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ShowDeletionQuestion()
        {
            try
            {
                if (SelectedDeviceGroupViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.DeviceGroupPage_ShowNotification);
                    return;
                }

                string question = string.Format(LanguageManager.GetLocalizationResource(Resource.MSG_DoUWantToDeleteObject), SelectedDeviceGroupViewModel.DeviceGroupName);
                Messenger.Default.Send(new NotificationMessageAction(this, question, Delete), Tokens.DeviceGroupPage_ShowQuestion);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Delete()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _deviceGroupRepo.Delete(SelectedDeviceGroupViewModel.DeviceGroupID);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteSuccessfully);

                    DeviceGroupViewModels.Remove(SelectedDeviceGroupViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteFail);
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DeviceGroupPage_ShowNotification);
            });
        }
    }
}
