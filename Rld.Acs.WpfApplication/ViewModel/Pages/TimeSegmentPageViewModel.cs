using System.Collections;
using System.Collections.ObjectModel;
using AutoMapper;
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
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class TimeSegmentPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITimeSegmentRepository _timeSegmentRepository = NinjectBinder.GetRepository<ITimeSegmentRepository>();
        private ITimeGroupRepository _timeGroupRepository = NinjectBinder.GetRepository<ITimeGroupRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public ObservableCollection<TimeSegmentViewModel> TimeSegmentViewModels { get; set; }
        public TimeSegmentViewModel SelectedTimeSegmentViewModel { get; set; }

        public TimeSegmentPageViewModel()
        {
            AddCmd = new AuthCommand(AddTimeSegment);
            ModifyCmd = new AuthCommand(ModifyTimeSegment);
            DeleteCmd = new AuthCommand(DeleteTimeSegment);

            var timeSegments = _timeSegmentRepository.Query(new Hashtable()).ToList();
            var viewmodels = timeSegments.Select(Mapper.Map<TimeSegmentViewModel>);
            TimeSegmentViewModels = new ObservableCollection<TimeSegmentViewModel>(viewmodels);
        }

        private void AddTimeSegment()
        {
            try
            {
                var viewModel = Mapper.Map<TimeSegmentViewModel>(new TimeSegment());

                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.OpenTimeSegmentView);
                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    viewModel = Mapper.Map<TimeSegmentViewModel>(viewModel.ViewModelAttachment.CoreModel);
                    TimeSegmentViewModels.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyTimeSegment()
        {
            try
            {
                if (SelectedTimeSegmentViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.TimeSegmentPage_ShowNotification);
                    return;
                }

                var coreModel = Mapper.Map<TimeSegment>(SelectedTimeSegmentViewModel);
                var viewModel = Mapper.Map<TimeSegmentViewModel>(coreModel);

                Messenger.Default.Send(new OpenWindowMessage() { DataContext = viewModel }, Tokens.OpenTimeSegmentView);
                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var index = TimeSegmentViewModels.IndexOf(SelectedTimeSegmentViewModel);
                    TimeSegmentViewModels[index] = viewModel;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteTimeSegment()
        {
            try
            {
                if (SelectedTimeSegmentViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_SelectValidData)), Tokens.TimeSegmentPage_ShowNotification);
                    return;
                }

                var timegroups = _timeGroupRepository.Query(new Hashtable());
                if (timegroups.Any())
                {
                    var timesegmentsInUsing = timegroups.SelectMany(x => x.TimeSegments);
                    if (timesegmentsInUsing.Any(x => x.TimeSegmentID == SelectedTimeSegmentViewModel.TimeSegmentID))
                    {
                        Messenger.Default.Send(new NotificationMessage(LanguageManager.GetLocalizationResource(Resource.MSG_CannotDeleteTimeSegmentBecauseOfTimeGroupAssociation)), Tokens.TimeSegmentPage_ShowNotification);
                        return;
                    }
                }

                string question = string.Format(LanguageManager.GetLocalizationResource(Resource.MSG_DoUWantToDeleteObject), SelectedTimeSegmentViewModel.TimeSegmentName);
                Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmDeleteTimeSegment), Tokens.TimeSegmentPage_ShowQuestion);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ConfirmDeleteTimeSegment()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _timeSegmentRepository.Delete(SelectedTimeSegmentViewModel.TimeSegmentID);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteSuccessfully);

                    TimeSegmentViewModels.Remove(SelectedTimeSegmentViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = LanguageManager.GetLocalizationResource(Resource.MSG_DeleteFail);
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeSegmentPage_ShowNotification);
            });
        }
    }
}
