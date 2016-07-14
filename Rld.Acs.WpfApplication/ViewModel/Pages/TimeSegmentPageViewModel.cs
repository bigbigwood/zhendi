using System.Collections;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Messages;
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
        public RelayCommand AddTimeSegmentCmd { get; private set; }
        public RelayCommand ModifyTimeSegmentCmd { get; private set; }
        public RelayCommand DeleteTimeSegmentCmd { get; private set; }
        public List<TimeSegment> TimeSegments { get; set; }
        public ObservableCollection<TimeSegmentViewModel> TimeSegmentViewModels { get; set; }
        public TimeSegmentViewModel SelectedTimeSegmentViewModel { get; set; }

        public TimeSegmentPageViewModel()
        {
            AddTimeSegmentCmd = new RelayCommand(AddTimeSegment);
            ModifyTimeSegmentCmd = new RelayCommand(ModifyTimeSegment);
            DeleteTimeSegmentCmd = new RelayCommand(DeleteTimeSegment);

            TimeSegmentViewModels = new ObservableCollection<TimeSegmentViewModel>();
            TimeSegments = _timeSegmentRepository.Query(new Hashtable()).ToList();
            TimeSegments.ForEach(t => TimeSegmentViewModels.Add(new TimeSegmentViewModel(t)));
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

                if (timeSegmentViewModel.ID != 0)
                    TimeSegmentViewModels.Add(timeSegmentViewModel);
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

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedTimeSegmentViewModel

                }, Tokens.OpenTimeSegmentView);

                //var vm = TimeSegmentViewModels.First(s => s.ID == SelectedTimeSegmentViewModel.ID);
                //vm = SelectedTimeSegmentViewModel;
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

                //if (AuthorizationDepartments.Any(d => d.Parent != null && d.Parent.DepartmentID == SelectedDepartmentDetailViewModel.CurrentDepartment.DepartmentID))
                //{
                //    Messenger.Default.Send(new NotificationMessage("选中部门存在子部门，请先删除所属子部门!"), Tokens.DepartmentPage_ShowNotification);
                //    return;
                //}

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
                    message = "删除时间段失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.DepartmentPage_ShowNotification);
            });
        }
    }
}
