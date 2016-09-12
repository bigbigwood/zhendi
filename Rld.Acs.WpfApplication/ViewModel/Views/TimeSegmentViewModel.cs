﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
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
            ViewModelAttachment = new ViewModelAttachment<TimeSegment>();
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
        public ViewModelAttachment<TimeSegment> ViewModelAttachment { get; set; }

        public TimeSegment CurrentTimeSegment { get; set; }

        private void Save()
        {
            string message = "";
            try
            {
                var validator = NinjectBinder.GetValidator<TimeSegmentViewModelValidator>();
                var results = validator.Validate(this);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                var beginDateTime = new DateTime(2000, 1, 1, StartHour.ToInt32(), StartMinute.ToInt32(), 0);
                var endDateTime = new DateTime(2000, 1, 1, EndHour.ToInt32(), EndMinute.ToInt32(), 0);
                if (endDateTime.Ticks - beginDateTime.Ticks < 0)
                {
                    message = "开始时间不能大于结束时间";
                    SendMessage(message);
                    return;
                }

                if (StartHour.Length == 1) StartHour = "0" + StartHour;
                if (EndHour.Length == 1) EndHour = "0" + EndHour;
                if (StartMinute.Length == 1) StartMinute = "0" + StartMinute;
                if (EndMinute.Length == 1) EndMinute = "0" + EndMinute;

                CurrentTimeSegment.BeginTime = string.Format("{0}:{1}", StartHour, StartMinute);
                CurrentTimeSegment.EndTime = string.Format("{0}:{1}", EndHour, EndMinute);
                CurrentTimeSegment.TimeSegmentName = Name;
                CurrentTimeSegment.Status = GeneralStatus.Enabled;

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

            ViewModelAttachment.CoreModel = CurrentTimeSegment;
            ViewModelAttachment.LastOperationSuccess = true;
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
