using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Validator;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class TimeGroupViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITimeGroupRepository _timeGroupRepo = NinjectBinder.GetRepository<ITimeGroupRepository>();
        private ITimeSegmentRepository _timeSegmentRepo = NinjectBinder.GetRepository<ITimeSegmentRepository>();

        public TimeGroupViewModel(TimeGroup timeGroup)
        {
            ViewModelAttachment = new ViewModelAttachment<TimeGroup>();
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            TimeSegmentDtos = new ObservableCollection<SelectableItem>();
            foreach (var timeSegment in AllTimeSegments)
            {
                TimeSegmentDtos.Add(new ListBoxItem { ID = timeSegment.TimeSegmentID, DisplayName = timeSegment.TimeSegmentName, IsSelected = false });
            }

            CurrentTimeGroup = timeGroup;
            if (timeGroup.TimeGroupID != 0)
            {
                TimeGroupID = timeGroup.TimeGroupID;
                Name = timeGroup.TimeGroupName;

                foreach (var timeSegment in timeGroup.TimeSegments)
                {
                    TimeSegmentDtos.First(t => t.ID == timeSegment.TimeSegmentID).IsSelected = true;
                }
            }
        }

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public string Title { get { return (TimeGroupID == 0) ? "新增时间组" : "修改时间组"; } }
        public Int32 TimeGroupID { get; set; }
        public string Name { get; set; }
        public TimeGroup CurrentTimeGroup { get; set; }
        public ObservableCollection<string> SelectedFormattingTimeSegmentList 
        { 
            get { return GetSelectedFormattingTimeSegmentList();}
        }
        public ObservableCollection<SelectableItem> TimeSegmentDtos { get; set; }

        public List<TimeSegment> AllTimeSegments
        {
            get { return _timeSegmentRepo.Query(new Hashtable { { "Status", "1" } }).ToList(); }
        }

        public ViewModelAttachment<TimeGroup> ViewModelAttachment { get; set; }

        private void Save()
        {
            string message = "";
            try
            {
                var validator = NinjectBinder.GetValidator<TimeGroupViewModelValidator>();
                var results = validator.Validate(this);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                CurrentTimeGroup.TimeGroupName = Name;
                CurrentTimeGroup.Status = GeneralStatus.Enabled;
                CurrentTimeGroup.TimeSegments = GetSelectedTimeSegments();

                if (CurrentTimeGroup.TimeGroupID == 0)
                {
                    CurrentTimeGroup.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentTimeGroup.CreateDate = DateTime.Now;
                    CurrentTimeGroup = _timeGroupRepo.Insert(CurrentTimeGroup);

                    message = "增加时间组成功!";
                }
                else
                {
                    CurrentTimeGroup.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentTimeGroup.UpdateDate = DateTime.Now;
                    _timeGroupRepo.Update(CurrentTimeGroup);

                    message = "修改时间组成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update department fails.", ex);
                message = "保存时间组失败";
                SendMessage(message);
                return;
            }

            ViewModelAttachment.CoreModel = CurrentTimeGroup;
            ViewModelAttachment.LastOperationSuccess = true;
            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseTimeGroupView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeGroupView_ShowNotification);
        }

        private List<TimeSegment> GetSelectedTimeSegments()
        {
            var selectedItems = TimeSegmentDtos.Where(t => t.IsSelected).ToList();
            return selectedItems.Select(item => AllTimeSegments.First(t => t.TimeSegmentID == item.ID)).ToList();
        }

        private ObservableCollection<string> GetSelectedFormattingTimeSegmentList()
        {
            var selected = new ObservableCollection<string>();
            var selectedTimeSegments = GetSelectedTimeSegments();
            foreach (var item in selectedTimeSegments)
            {
                selected.Add(string.Format("{0}-{1}", item.BeginTime, item.EndTime));
            }

            return selected;
        }
    }
}
