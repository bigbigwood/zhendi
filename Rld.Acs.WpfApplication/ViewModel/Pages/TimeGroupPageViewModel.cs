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
    public class TimeGroupPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITimeGroupRepository _timeGroupRepo = NinjectBinder.GetRepository<ITimeGroupRepository>();
        private ITimeSegmentRepository _timeSegmentRepo = NinjectBinder.GetRepository<ITimeSegmentRepository>();

        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }

        public ObservableCollection<TimeGroupViewModel> TimeGroupViewModels { get; set; }
        public TimeGroupViewModel SelectedTimeGroupViewModel { get; set; }

        public TimeGroupPageViewModel()
        {
            AddCmd = new AuthCommand(AddTimeGroup);
            ModifyCmd = new AuthCommand(ModifyTimeGroup);
            DeleteCmd = new AuthCommand(DeleteTimeGroup);

            var timegroups = _timeGroupRepo.Query(new Hashtable()).ToList();
            var viewModels = timegroups.Select(x => new TimeGroupViewModel(x));
            TimeGroupViewModels = new ObservableCollection<TimeGroupViewModel>(viewModels);
        }

        private void AddTimeGroup()
        {
            try
            {
                var timeGroupViewModel = new TimeGroupViewModel(new TimeGroup());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = timeGroupViewModel

                }, Tokens.OpenTimeGroupView);

                if (timeGroupViewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    TimeGroupViewModels.Add(timeGroupViewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyTimeGroup()
        {
            try
            {
                if (SelectedTimeGroupViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择时间组!"), Tokens.TimeGroupPage_ShowNotification);
                    return;
                }

                var viewModel = new TimeGroupViewModel(SelectedTimeGroupViewModel.CurrentTimeGroup);
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenTimeGroupView);

                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var index = TimeGroupViewModels.IndexOf(SelectedTimeGroupViewModel);
                    TimeGroupViewModels[index] = viewModel;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteTimeGroup()
        {
            try
            {
                if (SelectedTimeGroupViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择时间组!"), Tokens.TimeGroupPage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除时间组:{0}吗？", SelectedTimeGroupViewModel.Name);
                Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmDeleteTimeGroup), Tokens.TimeGroupPage_ShowQuestion);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ConfirmDeleteTimeGroup()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _timeGroupRepo.Delete(SelectedTimeGroupViewModel.CurrentTimeGroup.TimeGroupID);
                    message = "删除时间组成功!";

                    TimeGroupViewModels.Remove(SelectedTimeGroupViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除时间组失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeGroupPage_ShowNotification);
            });
        }

    }
}
