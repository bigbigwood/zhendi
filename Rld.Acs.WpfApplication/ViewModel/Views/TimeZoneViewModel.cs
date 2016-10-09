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
using Rld.Acs.Unility.Exceptions;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Validator;
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
            ViewModelAttachment = new ViewModelAttachment<RldModel.TimeZone>();
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            CurrentTimeZone = timeZone;
            if (timeZone.TimeZoneID != 0)
            {
                ID = timeZone.TimeZoneID;
                Name = timeZone.TimeZoneName;
                Code = timeZone.TimeZoneCode;
            }

            Title = (timeZone.TimeZoneID == 0) ? "新增时间区" : "修改时间区";
            TimeGroupAssociationsDtos = InitTimeZoneGroupDtos();
        }

        public string Title { get; set; }
        public Int32 ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Boolean IsEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RldModel.TimeZone CurrentTimeZone { get; set; }
        public ViewModelAttachment<RldModel.TimeZone> ViewModelAttachment { get; set; }
        public ObservableCollection<TimeZoneGroupMappingInfo> TimeGroupAssociationsDtos { get; set; }

        public List<TimeGroup> AllTimeGroups
        {
            get { return _timeGroupRepo.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled); }
        }

        private void Save()
        {
            string message = "";
            try
            {
                var validator = NinjectBinder.GetValidator<TimeZoneViewModelValidator>();
                var results = validator.Validate(this);
                if (!results.IsValid)
                {
                    message = string.Join("\n", results.Errors);
                    SendMessage(message);
                    return;
                }

                CurrentTimeZone.TimeZoneName = Name;
                CurrentTimeZone.TimeZoneCode = Code;
                CurrentTimeZone.Status = GeneralStatus.Enabled;
                CurrentTimeZone.TimeGroupAssociations = GetTimeGroupAssociations();

                if (CurrentTimeZone.TimeZoneID == 0)
                {
                    CurrentTimeZone.CreateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentTimeZone.CreateDate = DateTime.Now;
                    CurrentTimeZone = _timeZoneRepo.Insert(CurrentTimeZone);
                    message = "增加时间区成功!";
                }
                else
                {
                    CurrentTimeZone.UpdateUserID = ApplicationManager.GetInstance().CurrentOperatorInfo.OperatorID;
                    CurrentTimeZone.UpdateDate = DateTime.Now;
                    _timeZoneRepo.Update(CurrentTimeZone);
                    message = "修改时间区成功!";
                }
            }
            catch (BusinessException ex)
            {
                Log.Error("Update timezone fails.", ex);
                SendMessage(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Log.Error("Update timezone fails.", ex);
                message = "保存时间区失败";
                SendMessage(message);
                return;
            }

            ViewModelAttachment.CoreModel = CurrentTimeZone;
            ViewModelAttachment.LastOperationSuccess = true;
            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.CloseTimeZoneView);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeZoneView_ShowNotification);
        }

        private ObservableCollection<TimeZoneGroupMappingInfo> InitTimeZoneGroupDtos()
        {
            var dtos = new ObservableCollection<TimeZoneGroupMappingInfo>();
            dtos.Add(BuildMappingInfo(CurrentTimeZone, 1));
            dtos.Add(BuildMappingInfo(CurrentTimeZone, 2));
            dtos.Add(BuildMappingInfo(CurrentTimeZone, 3));
            dtos.Add(BuildMappingInfo(CurrentTimeZone, 4));
            dtos.Add(BuildMappingInfo(CurrentTimeZone, 5));
            dtos.Add(BuildMappingInfo(CurrentTimeZone, 6));
            dtos.Add(BuildMappingInfo(CurrentTimeZone, 7));
            return dtos;
        }
        private TimeZoneGroupMappingInfo BuildMappingInfo(RldModel.TimeZone timeZone, int index)
        {
            int id = 0;
            string name = "";
            TimeGroup timeGroup = new TimeGroup();

            var association = timeZone.TimeGroupAssociations.FirstOrDefault(t => t.DisplayOrder == index);
            if (association != null)
            {
                id = association.TimeZoneGroupID;
                timeGroup = AllTimeGroups.FirstOrDefault(t => t.TimeGroupID == association.TimeGroupID);
            }
            else
            {
                timeGroup = AllTimeGroups.First();
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
                SelectedTimeGroupName = timeGroup.TimeGroupName,
                TimeGroupViewModel = new TimeGroupViewModel(timeGroup),
            };
        }

        private List<TimeZoneGroup> GetTimeGroupAssociations()
        {
            var result = new List<TimeZoneGroup>();
            foreach (var item in TimeGroupAssociationsDtos)
            {
                var timegroup = AllTimeGroups.First(t => t.TimeGroupName == item.SelectedTimeGroupName);
                result.Add(new TimeZoneGroup()
                {
                    TimeZoneGroupID = item.ID, 
                    DisplayOrder = item.DisplayOrder, 
                    MappingName = item.Name,
                    TimeZoneID = this.ID, 
                    TimeGroupID = timegroup.TimeGroupID,
                });
            }

            return result;
        }
    }
}
