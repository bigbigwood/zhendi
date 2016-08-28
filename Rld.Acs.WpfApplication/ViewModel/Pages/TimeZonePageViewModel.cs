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
        private ITimeGroupRepository _timeGroupRepo = NinjectBinder.GetRepository<ITimeGroupRepository>();
        public RelayCommand AddTimeZoneCmd { get; private set; }
        public RelayCommand ModifyTimeZoneCmd { get; private set; }
        public RelayCommand DeleteTimeZoneCmd { get; private set; }
        public RelayCommand TimeZoneDashboardCmd { get; private set; }

        public ObservableCollection<TimeZoneViewModel> TimeZoneViewModels { get; set; }
        public TimeZoneViewModel SelectedTimeZoneViewModel { get; set; }

        public TimeZonePageViewModel()
        {
            AddTimeZoneCmd = new AuthCommand(AddTimeZone);
            ModifyTimeZoneCmd = new AuthCommand(ModifyTimeZone);
            DeleteTimeZoneCmd = new AuthCommand(DeleteTimeZone);
            TimeZoneDashboardCmd = new AuthCommand(OpenTimeZoneDetail);

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

                if (timeZoneViewModel.CurrentTimeZone.TimeZoneID != 0)
                    TimeZoneViewModels.Add(timeZoneViewModel);
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

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedTimeZoneViewModel

                }, Tokens.OpenTimeZoneView);
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

                //if (AuthorizationDepartments.Any(d => d.Parent != null && d.Parent.DepartmentID == SelectedDepartmentDetailViewModel.CurrentDepartment.DepartmentID))
                //{
                //    Messenger.Default.Send(new NotificationMessage("选中部门存在子部门，请先删除所属子部门!"), Tokens.DepartmentPage_ShowNotification);
                //    return;
                //}

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
                Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeGroupPage_ShowNotification);
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
