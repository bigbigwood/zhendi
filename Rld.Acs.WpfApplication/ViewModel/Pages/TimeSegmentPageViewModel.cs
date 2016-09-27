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

            var TimeSegments = _timeSegmentRepository.Query(new Hashtable()).ToList();
            var viewmodels = TimeSegments.Select(x => new TimeSegmentViewModel(x));
            TimeSegmentViewModels = new ObservableCollection<TimeSegmentViewModel>(viewmodels);
        }

        private void AddTimeSegment()
        {
            try
            {
                var timeSegmentViewModel = new TimeSegmentViewModel(new TimeSegment());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = timeSegmentViewModel

                }, Tokens.OpenTimeSegmentView);

                if (timeSegmentViewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    TimeSegmentViewModels.Add(timeSegmentViewModel);
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
                    Messenger.Default.Send(new NotificationMessage("请先选择时间段!"), Tokens.TimeSegmentPage_ShowNotification);
                    return;
                }

                var viewModel = new TimeSegmentViewModel(SelectedTimeSegmentViewModel.CurrentTimeSegment);
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenTimeSegmentView);

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
                    Messenger.Default.Send(new NotificationMessage("请先选择时间段!"), Tokens.TimeSegmentPage_ShowNotification);
                    return;
                }

                var timegroups = _timeGroupRepository.Query(new Hashtable());
                if (timegroups.Any())
                {
                    var timesegmentsInUsing = timegroups.SelectMany(x => x.TimeSegments);
                    if (timesegmentsInUsing.Any(x => x.TimeSegmentID == SelectedTimeSegmentViewModel.ID))
                    {
                        Messenger.Default.Send(new NotificationMessage("该时间段已经被关联到时间组，不能删除!"), Tokens.TimeSegmentPage_ShowNotification);
                        return;
                    }
                }

                string question = string.Format("确定删除时间段:{0}吗？", SelectedTimeSegmentViewModel.Name);
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
                    _timeSegmentRepository.Delete(SelectedTimeSegmentViewModel.CurrentTimeSegment.TimeSegmentID);
                    message = "删除时间段成功!";

                    TimeSegmentViewModels.Remove(SelectedTimeSegmentViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = string.Format("删除时间段失败!\n{0}", ex.Message);
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeSegmentPage_ShowNotification);
            });
        }
    }
}
