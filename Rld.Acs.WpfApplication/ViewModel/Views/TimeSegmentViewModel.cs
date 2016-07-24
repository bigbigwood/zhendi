using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Validator;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class TimeSegmentViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITimeSegmentRepository _timeSegmentRepo = NinjectBinder.GetRepository<ITimeSegmentRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public TimeSegmentViewModel(TimeSegment timeSegment)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            CurrentTimeSegment = timeSegment;
            if (timeSegment.TimeSegmentID != 0)
            {
                ID = timeSegment.TimeSegmentID;
                Name = timeSegment.TimeSegmentName;
                StartHour = timeSegment.BeginTime.Substring(0, 2);
                StartMinute = timeSegment.BeginTime.Substring(3, 2);
                EndHour = timeSegment.EndTime.Substring(0, 2);
                EndMinute = timeSegment.EndTime.Substring(3, 2);
            }

            Title = (timeSegment.TimeSegmentID == 0) ? "新增时间段" : "修改时间段";
        }

        public string Title { get; set; }
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public string StartHour { get; set; }
        public string StartMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }
        public string FullSegment
        {
            get { return string.Format("{0}-{1}", CurrentTimeSegment.BeginTime, CurrentTimeSegment.EndTime); }
        }

        public TimeSegment CurrentTimeSegment { get; set; }

        private void Save()
        {
            string message = "";
            try
            {
                if (StartHour.Length == 1) StartHour = "0" + StartHour;
                if (EndHour.Length == 1) StartHour = "0" + EndHour;

                CurrentTimeSegment.BeginTime = string.Format("{0}:{1}", StartHour, StartMinute);
                CurrentTimeSegment.EndTime = string.Format("{0}:{1}", EndHour, EndMinute);
                CurrentTimeSegment.TimeSegmentName = Name;
                CurrentTimeSegment.Status = GeneralStatus.Enabled;

                var validator = NinjectBinder.GetValidator<TimeSegmentValidator>();
                var results = validator.Validate(CurrentTimeSegment);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                if (CurrentTimeSegment.TimeSegmentID == 0)
                {
                    CurrentTimeSegment.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentTimeSegment.CreateDate = DateTime.Now;
                    CurrentTimeSegment = _timeSegmentRepo.Insert(CurrentTimeSegment);

                    message = "增加时间段成功!";
                }
                else
                {
                    CurrentTimeSegment.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentTimeSegment.UpdateDate = DateTime.Now;
                    _timeSegmentRepo.Update(CurrentTimeSegment);

                    message = "修改时间段成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update department fails.", ex);
                message = "保存时间段失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseTimeSegmentView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeSegmentView_ShowNotification);
        }
    }
}
