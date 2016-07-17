using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

            AllTimeGroups = _timeGroupRepo.Query(new Hashtable { { "Status", "1" } }).ToList();

            CurrentTimeZone = timeZone;
            if (timeZone.TimeZoneID != 0)
            {
                ID = timeZone.TimeZoneID;
                Name = timeZone.TimeZoneName;
            }

            var allTimeGroupDtos = GetAllTimeGroupDtos(AllTimeGroups);
            TimeGroupAssociationsDtos = new ObservableCollection<TimeZoneGroupMappingInfo>();
            TimeGroupAssociationsDtos.Add(BuildMappingInfo(timeZone, allTimeGroupDtos, 1));
            TimeGroupAssociationsDtos.Add(BuildMappingInfo(timeZone, allTimeGroupDtos, 2));
            TimeGroupAssociationsDtos.Add(BuildMappingInfo(timeZone, allTimeGroupDtos, 3));
            TimeGroupAssociationsDtos.Add(BuildMappingInfo(timeZone, allTimeGroupDtos, 4));
            TimeGroupAssociationsDtos.Add(BuildMappingInfo(timeZone, allTimeGroupDtos, 5));
            TimeGroupAssociationsDtos.Add(BuildMappingInfo(timeZone, allTimeGroupDtos, 6));
            TimeGroupAssociationsDtos.Add(BuildMappingInfo(timeZone, allTimeGroupDtos, 7));

            Title = (timeZone.TimeZoneID == 0) ? "新增时间组" : "修改时间组";
        }

        public string Title { get; set; }
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public Boolean IsEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RldModel.TimeZone CurrentTimeZone { get; set; }
        public ObservableCollection<TimeZoneGroupMappingInfo> TimeGroupAssociationsDtos { get; set; }
        public List<TimeGroup> AllTimeGroups { get; set; }

        private void Save()
        {
            string message = "";
            try
            {
                if (CurrentTimeZone.TimeZoneID == 0)
                {
                    CurrentTimeZone.TimeZoneName = Name;
                    CurrentTimeZone.Status = GeneralStatus.Enabled;
                    CurrentTimeZone.CreateUserID = 1;
                    CurrentTimeZone.CreateDate = DateTime.Now;
                    //CurrentTimeZone.TimeSegments = GetSelectedTimeSegments();
                    CurrentTimeZone = _timeZoneRepo.Insert(CurrentTimeZone);

                    message = "增加时间区成功!";
                }
                else
                {
                    CurrentTimeZone.TimeZoneName = Name;
                    CurrentTimeZone.Status = GeneralStatus.Enabled;
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

        private TimeZoneGroupMappingInfo BuildMappingInfo(RldModel.TimeZone timeZone, List<ComboBoxItem> allTimeGroupDtos, int index)
        {
            int id = 0;
            string name = "";
            TimeGroup timeGroup = new TimeGroup();
            ComboBoxItem selectedTimeGroupDto = new ComboBoxItem();

            var association = timeZone.TimeGroupAssociations.FirstOrDefault(t => t.DisplayOrder == index);
            if (association != null)
            {
                id = association.TimeZoneGroupID;
                selectedTimeGroupDto = allTimeGroupDtos.FirstOrDefault(t => t.ID == association.TimeZoneGroupID);
            }


            switch (index)
            {
                case 1: name = "星期一"; break;
                case 2: name = "星期二"; break;
                case 3: name = "星期三"; break;
                case 4: name = "星期四"; break;
                case 5: name = "星期五"; break;
                case 6: name = "星期六"; break;
                case 7: name = "星期日"; break;
            }

            return new TimeZoneGroupMappingInfo()
            {
                ID = id,
                DisplayOrder = index,
                Name = name,
                AllTimeGroupDtos = allTimeGroupDtos,
                SelectedTimeGroupDto = selectedTimeGroupDto,
            };
        }

        private List<ComboBoxItem> GetAllTimeGroupDtos(IEnumerable<TimeGroup> allTimeGroups)
        {
            Debug.Assert(allTimeGroups != null, "allTimeGroups != null");
            var result = new List<ComboBoxItem>();
            foreach (var timegroup in allTimeGroups)
            {
                result.Add(new ComboBoxItem(){IsSelected = false, ID = timegroup.TimeGroupID, DisplayName = timegroup.TimeGroupName});
            }
            return result;
        }
    }
}
