using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class TimeZonePageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITimeZoneRepository _timeZoneRepo = NinjectBinder.GetRepository<ITimeZoneRepository>();
        public RelayCommand AddCmd { get; private set; }
        public RelayCommand ModifyCmd { get; private set; }
        public RelayCommand DeleteCmd { get; private set; }
        public RelayCommand TimeZoneDashboardCmd { get; private set; }

        public ObservableCollection<TimeZoneViewModel> TimeZoneViewModels { get; set; }
        public TimeZoneViewModel SelectedTimeZoneViewModel { get; set; }

        public TimeZonePageViewModel()
        {
            AddCmd = new AuthCommand(AddTimeZone);
            ModifyCmd = new AuthCommand(ModifyTimeZone);
            DeleteCmd = new AuthCommand(DeleteTimeZone);
            TimeZoneDashboardCmd = new RelayCommand(OpenTimeZoneDetail);

            TimeZoneViewModels = new ObservableCollection<TimeZoneViewModel>();
            var timezones = _timeZoneRepo.Query(new Hashtable()).ToList();
            timezones.ForEach(t => TimeZoneViewModels.Add(new TimeZoneViewModel(t)));
        }

        private void AddTimeZone()
        {
            try
            {
                var timeZoneViewModel = new TimeZoneViewModel(new Model.TimeZone());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = timeZoneViewModel

                }, Tokens.OpenTimeZoneView);

                if (timeZoneViewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    TimeZoneViewModels.Add(timeZoneViewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyTimeZone()
        {
            try
            {
                if (SelectedTimeZoneViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择时间区!"), Tokens.TimeZonePage_ShowNotification);
                    return;
                }

                var viewModel = new TimeZoneViewModel(SelectedTimeZoneViewModel.CurrentTimeZone);
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = viewModel

                }, Tokens.OpenTimeZoneView);

                if (viewModel.ViewModelAttachment.LastOperationSuccess)
                {
                    var index = TimeZoneViewModels.IndexOf(SelectedTimeZoneViewModel);
                    TimeZoneViewModels[index] = viewModel;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }


        private void DeleteTimeZone()
        {
            try
            {
                if (SelectedTimeZoneViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择时间区!"), Tokens.TimeZonePage_ShowNotification);
                    return;
                }

                string question = string.Format("确定删除时间区:{0}吗？", SelectedTimeZoneViewModel.Name);
                Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmDeleteTimeZone), Tokens.TimeZonePage_ShowQuestion);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void ConfirmDeleteTimeZone()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _timeZoneRepo.Delete(SelectedTimeZoneViewModel.CurrentTimeZone.TimeZoneID);
                    message = "删除时间区成功!";

                    TimeZoneViewModels.Remove(SelectedTimeZoneViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除时间区失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeZonePage_ShowNotification);
            });
        }


        private void OpenTimeZoneDetail()
        {
            try
            {
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedTimeZoneViewModel

                }, Tokens.OpenTimeZoneDashboardView);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
