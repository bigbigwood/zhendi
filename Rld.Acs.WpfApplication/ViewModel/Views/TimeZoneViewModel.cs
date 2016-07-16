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
using RldModel = Rld.Acs.Model;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class TimeZoneViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITimeZoneRepository _timeZoneRepo = NinjectBinder.GetRepository<ITimeZoneRepository>();
        private ITimeGroupRepository _timeGroupRepo = NinjectBinder.GetRepository<ITimeGroupRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }

        public TimeZoneViewModel(RldModel.TimeZone timeZone)
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            TimeGroupDtos = new ObservableCollection<SelectableItem>();
            AllTimeGroups = _timeGroupRepo.Query(new Hashtable { { "Status", "1" } }).ToList();
            foreach (var timegroup in AllTimeGroups)
            {
                TimeGroupDtos.Add(new ListBoxItem { IsSelected = false, ID = timegroup.TimeGroupID, DisplayName = timegroup.TimeGroupName});
            }

            CurrentTimeZone = timeZone;
            if (timeZone.TimeZoneID != 0)
            {
                ID = timeZone.TimeZoneID;
                Name = timeZone.TimeZoneName;

                foreach (var timegroup in timeZone.TimeGroups)
                {
                    TimeGroupDtos.First(t => t.ID == timegroup.TimeGroupID).IsSelected = true;
                }
            }

            Title = (timeZone.TimeZoneID == 0) ? "新增时间组" : "修改时间组";
        }

        public string Title { get; set; }
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public Boolean IsEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RldModel.TimeZone CurrentTimeZone { get; set; }
        public ObservableCollection<SelectableItem> TimeGroupDtos { get; set; }
        public List<TimeGroup> AllTimeGroups { get; set; }

        private void Save()
        {
            string message = "";
            try
            {
                if (CurrentTimeZone.TimeZoneID == 0)
                {
                    CurrentTimeZone.TimeZoneName = Name;
                    CurrentTimeZone.Status = IsEnabled ? GeneralStatus.Enabled : GeneralStatus.Disabled;
                    CurrentTimeZone.CreateUserID = 1;
                    CurrentTimeZone.CreateDate = DateTime.Now;
                    //CurrentTimeZone.TimeSegments = GetSelectedTimeSegments();
                    CurrentTimeZone = _timeZoneRepo.Insert(CurrentTimeZone);

                    message = "增加时间区成功!";
                }
                else
                {
                    CurrentTimeZone.TimeZoneName = Name;
                    CurrentTimeZone.Status = IsEnabled ? GeneralStatus.Enabled : GeneralStatus.Disabled;
                    CurrentTimeZone.UpdateUserID = 1;
                    CurrentTimeZone.UpdateDate = DateTime.Now;
                    //CurrentTimeZone.TimeSegments = GetSelectedTimeSegments();
                    _timeZoneRepo.Update(CurrentTimeZone);

                    RaisePropertyChanged(null);
                    message = "修改时间区成功!";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update department fails.", ex);
                message = "保存时间区失败";
                return;
            }

            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseTimeZoneView);
        }

        //private List<TimeSegment> GetSelectedTimeSegments()
        //{
        //    var selectedItems = TimeSegmentDtos.Where(t => t.IsSelected).ToList();
        //    return selectedItems.Select(item => AllTimeSegments.First(t => t.TimeSegmentID == item.ID)).ToList();
        //}
    }
}
