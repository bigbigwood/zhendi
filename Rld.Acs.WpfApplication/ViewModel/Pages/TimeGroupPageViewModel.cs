﻿using System.Collections;
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
    public class TimeGroupPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITimeGroupRepository _timeGroupRepo = NinjectBinder.GetRepository<ITimeGroupRepository>();
        private ITimeSegmentRepository _timeSegmentRepo = NinjectBinder.GetRepository<ITimeSegmentRepository>();

        public RelayCommand AddTimeGroupCmd { get; private set; }
        public RelayCommand ModifyTimeGroupCmd { get; private set; }
        public RelayCommand DeleteTimeGroupCmd { get; private set; }

        public ObservableCollection<TimeGroupViewModel> TimeGroupViewModels { get; set; }
        public TimeGroupViewModel SelectedTimeGroupViewModel { get; set; }

        public TimeGroupPageViewModel()
        {
            AddTimeGroupCmd = new RelayCommand(AddTimeGroup);
            ModifyTimeGroupCmd = new RelayCommand(ModifyTimeGroup);
            DeleteTimeGroupCmd = new RelayCommand(DeleteTimeGroup);

            TimeGroupViewModels = new ObservableCollection<TimeGroupViewModel>();
            var timegroups = _timeGroupRepo.Query(new Hashtable()).ToList();
            timegroups.ForEach(t => TimeGroupViewModels.Add(new TimeGroupViewModel(t)));
        }

        private void AddTimeGroup()
        {
            try
            {
                var timeGroupViewModel = new TimeGroupViewModel(new TimeGroup());
                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = timeGroupViewModel

                }, Tokens.OpenTimeGroupView);

                if (timeGroupViewModel.CurrentTimeGroup.TimeGroupID!= 0)
                    TimeGroupViewModels.Add(timeGroupViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ModifyTimeGroup()
        {
            try
            {
                if (SelectedTimeGroupViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择时间组!"), Tokens.TimeGroupPage_ShowNotification);
                    return;
                }

                Messenger.Default.Send(new OpenWindowMessage()
                {
                    DataContext = SelectedTimeGroupViewModel

                }, Tokens.OpenTimeGroupView);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void DeleteTimeGroup()
        {
            try
            {
                if (SelectedTimeGroupViewModel == null)
                {
                    Messenger.Default.Send(new NotificationMessage("请先选择时间组!"), Tokens.TimeGroupPage_ShowNotification);
                    return;
                }

                //if (AuthorizationDepartments.Any(d => d.Parent != null && d.Parent.DepartmentID == SelectedDepartmentDetailViewModel.CurrentDepartment.DepartmentID))
                //{
                //    Messenger.Default.Send(new NotificationMessage("选中部门存在子部门，请先删除所属子部门!"), Tokens.DepartmentPage_ShowNotification);
                //    return;
                //}

                string question = string.Format("确定删除时间组:{0}吗？", SelectedTimeGroupViewModel.Name);
                Messenger.Default.Send(new NotificationMessageAction(this, question, ConfirmDeleteTimeGroup), Tokens.TimeGroupPage_ShowQuestion);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void ConfirmDeleteTimeGroup()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                string message = "";
                try
                {
                    _timeGroupRepo.Delete(SelectedTimeGroupViewModel.CurrentTimeGroup.TimeGroupID);
                    message = "删除时间组成功!";

                    TimeGroupViewModels.Remove(SelectedTimeGroupViewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    message = "删除时间组失败！";
                }
                Messenger.Default.Send(new NotificationMessage(message), Tokens.TimeGroupPage_ShowNotification);
            });
        }

    }
}
