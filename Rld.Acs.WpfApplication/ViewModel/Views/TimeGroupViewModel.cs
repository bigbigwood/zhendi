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
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;

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

            Title = (timeGroup.TimeGroupID == 0) ? "新增时间组" : "修改时间组";
        }

        public string Title { get; set; }
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public TimeGroup CurrentTimeGroup { get; set; }
        public ObservableCollection<SelectableItem> TimeSegmentDtos { get; set; }
        public List<TimeSegment> AllTimeSegments { get; set; }

        private void Save()
        {
            string message = "";
            try
            {
                if (CurrentTimeGroup.TimeGroupID == 0)
                {
                    CurrentTimeGroup.TimeGroupName = Name;
                    CurrentTimeGroup.Status = GeneralStatus.Enabled;
                    CurrentTimeGroup.CreateUserID = 1;
                    CurrentTimeGroup.CreateDate = DateTime.Now;
                    CurrentTimeGroup.TimeSegments = GetSelectedTimeSegments();
                    CurrentTimeGroup = _timeGroupRepo.Insert(CurrentTimeGroup);

                    message = "增加时间组成功!";
                }
                else
                {
                    CurrentTimeGroup.TimeGroupName = Name;
                    CurrentTimeGroup.Status = GeneralStatus.Enabled;
                    CurrentTimeGroup.UpdateUserID = 1;
                    CurrentTimeGroup.UpdateDate = DateTime.Now;
                    CurrentTimeGroup.TimeSegments = GetSelectedTimeSegments();
                    _timeGroupRepo.Update(CurrentTimeGroup);

                    RaisePropertyChanged(null);
                    message = "修改时间组成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update department fails.", ex);
                message = "保存时间组失败";
                return;
            }

            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseTimeGroupView);
        }

        private List<TimeSegment> GetSelectedTimeSegments()
        {
            var selectedItems = TimeSegmentDtos.Where(t => t.IsSelected).ToList();
            return selectedItems.Select(item => AllTimeSegments.First(t => t.TimeSegmentID == item.ID)).ToList();
        }
    }
}
