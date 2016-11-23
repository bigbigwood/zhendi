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
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.ViewModel.Views;
using GalaSoft.MvvmLight.Threading;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class SysOperatorPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysOperatorRepository _sysOperatorRepo = NinjectBinder.GetRepository<ISysOperatorRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public ObservableCollection<SysOperatorViewModel> SysOperatorViewModels { get; set; }
        public SysOperatorViewModel SelectedSysOperatorViewModel { get; set; }


        public SysOperatorPageViewModel()
        {
            AddCmd = new AuthCommand(Add);
            ModifyCmd = new AuthCommand(Modify);
            DeleteCmd = new AuthCommand(ShowDeletionQuestion);

            var operators = _sysOperatorRepo.Query(new Hashtable());
            var vms = operators.Select(Mapper.Map<SysOperatorViewModel>);
            SysOperatorViewModels = new ObservableCollection<SysOperatorViewModel>(vms);
        }

        private void Add()
        {
            try
            {
                var viewModel = Mapper.Map<SysOperatorViewModel>(new SysOperator());
                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.SysOperatorView_Open);
                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    viewModel = Mapper.Map<SysOperatorViewModel>(viewModel.ViewModelAttachment.CoreModel);
                    SysOperatorViewModels.Add(viewModel);
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
                if (SelectedSysOperatorViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.SysOperatorPage_ShowNotification);
                    return;
                }

                var coreModel = Mapper.Map<SysOperator>(SelectedSysOperatorViewModel);
                var viewModel = Mapper.Map<SysOperatorViewModel>(coreModel);

                //SelectedSysOperatorViewModel.NewPasswordEnabled = false;
                ////SelectedSysOperatorViewModel.NewPassword1 = ""; 
                ////SelectedSysOperatorViewModel.NewPassword2 = ""; // 会导致界面输入的值没办法在viewmodel获取到
                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.SysOperatorView_Open);
                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var index = SysOperatorViewModels.IndexOf(SelectedSysOperatorViewModel);
                    SysOperatorViewModels[index] = viewModel;
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
                if (SelectedSysOperatorViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.SysOperatorPage_ShowNotification);
                    return;
                }

                string question = string.Format(LanguageManager.GetLocalizationResource(Resource.MSG_DoUWantToDeleteObject), SelectedSysOperatorViewModel.LoginName);
                Messenger.Default.Send(new NotificationMessageAction(this, question, Delete), Tokens.SysOperatorPage_ShowQuestion);
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
                    _sysOperatorRepo.Delete(SelectedSysOperatorViewModel.OperatorID);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteSuccessfully);

                    SysOperatorViewModels.Remove(SelectedSysOperatorViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteFail);
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.SysOperatorPage_ShowNotification);
            });
        }
    }
}
