using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using AutoMapper;
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

        public Int32 TimeSegmentID { get; set; }
        public String TimeSegmentName { get; set; }
        public String TimeSegmentCode { get; set; }
        public String BeginTime { get; set; }
        public String EndTime { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string StartHour { get; set; }
        public string StartMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }
        public string FullSegment
        {
            get { return string.Format("{0}-{1}", BeginTime, EndTime); }
        }
        public string Title { get { return (TimeSegmentID == 0) ? "新增时间段" : "修改时间段"; } }
        public ViewModelAttachment<TimeSegment> ViewModelAttachment { get; set; }
        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public TimeSegmentViewModel()
        {
            ViewModelAttachment = new ViewModelAttachment<TimeSegment>();
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
        }


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

                BeginTime = beginDateTime.ToString("HH:mm");
                EndTime = endDateTime.ToString("HH:mm");
                Status = GeneralStatus.Enabled;

                var coreModel = Mapper.Map<TimeSegment>(this);

                if (TimeSegmentID == 0)
                {
                    coreModel.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    coreModel.CreateDate = DateTime.Now;
                    coreModel = _timeSegmentRepo.Insert(coreModel);
                    message = "增加时间段成功!";
                }
                else
                {
                    coreModel.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    coreModel.UpdateDate = DateTime.Now;
                    _timeSegmentRepo.Update(coreModel);
                    message = "修改时间段成功!";
                }

                ViewModelAttachment.CoreModel = coreModel;
                ViewModelAttachment.LastOperationSuccess = true;
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
