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

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public TimeGroupViewModel(TimeGroup timeGroup)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            TimeSegmentDtos = new ObservableCollection<SelectableItem>();
            AllTimeSegments = _timeSegmentRepo.Query(new Hashtable { { "Status", "1" } }).ToList();
            foreach (var timeSegment in AllTimeSegments)
            {
                TimeSegmentDtos.Add(new ListBoxItem { ID = timeSegment.TimeSegmentID, DisplayName = timeSegment.TimeSegmentName, IsSelected = false });
            }

            CurrentTimeGroup = timeGroup;
            if (timeGroup.TimeGroupID != 0)
            {
                ID = timeGroup.TimeGroupID;
                Name = timeGroup.TimeGroupName;

                foreach (var timeSegment in timeGroup.TimeSegments)
                {
                    TimeSegmentDtos.First(t => t.ID == timeSegment.TimeSegmentID).IsSelected = true;
                }
            }

            SelectedFormattingTimeSegmentList = GetSelectedFormattingTimeSegmentList();

            Title = (timeGroup.TimeGroupID == 0) ? "新增时间组" : "修改时间组";
        }

        public string Title { get; set; }
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public TimeGroup CurrentTimeGroup { get; set; }
        public ObservableCollection<string> SelectedFormattingTimeSegmentList { get; set; }
        public ObservableCollection<SelectableItem> TimeSegmentDtos { get; set; }
        public List<TimeSegment> AllTimeSegments { get; set; }

        private void Save()
        {
            string message = "";
            try
            {
                CurrentTimeGroup.TimeGroupName = Name;
                CurrentTimeGroup.Status = GeneralStatus.Enabled;
                CurrentTimeGroup.TimeSegments = GetSelectedTimeSegments();

                var validator = NinjectBinder.GetValidator<TimeGroupValidator>();
                var results = validator.Validate(CurrentTimeGroup);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

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

            SelectedFormattingTimeSegmentList = GetSelectedFormattingTimeSegmentList();

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
