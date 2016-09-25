using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service;
using Rld.Acs.WpfApplication.Service.Validator;
using TimeZone = Rld.Acs.Model.TimeZone;

namespace Rld.Acs.WpfApplication.ViewModel.Views
{
    public class FloorViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IFloorRepository _floorRepo = NinjectBinder.GetRepository<IFloorRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public RelayCommand CancelCmd { get; private set; }
        public ViewModelAttachment<Floor> ViewModelAttachment { get; set; }

        public ObservableCollection<FloorDoorViewModel> Doors { get; set; }
        public virtual Int32 FloorID { get; set; }
        public String Name { get; set; }
        public String Photo { get; set; }
        public GeneralStatus Status { get; set; }

        public string DoorNames
        {
            get
            {
                var names = Doors.FindAll(x => x.FloorID == FloorID).Select(x => x.DoorName);
                return string.Join(", ", names);
            }
        }

        public FloorViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));
            ViewModelAttachment = new ViewModelAttachment<Floor>();

            InitDoorListBox();
        }

        public void InitDoorListBox()
        {
            Doors = new ObservableCollection<FloorDoorViewModel>();
            foreach (var authDoor in FloorDoorManager.GetInstance().AuthorizationDoors)
            {
                var floorDoorViewModel = new FloorDoorViewModel();
                var bindedDoor = FloorDoorManager.GetInstance().AuthorizationFloorDoor.FirstOrDefault(x => x.DoorID == authDoor.DeviceDoorID);
                if (bindedDoor != null)
                {
                    floorDoorViewModel = Mapper.Map<FloorDoorViewModel>(bindedDoor);
                    floorDoorViewModel.DoorName = authDoor.Name;
                    floorDoorViewModel.Enabled = (bindedDoor.FloorDoorID == 0 || bindedDoor.FloorID == FloorID);
                }
                else
                {
                    floorDoorViewModel.DoorID = authDoor.DeviceDoorID;
                    floorDoorViewModel.DoorName = authDoor.Name;
                    floorDoorViewModel.Enabled = true;
                }
                Doors.Add(floorDoorViewModel);
            }
        }

        public void BindDoors(List<FloorDoor> coreDoors)
        {
            coreDoors.ForEach(x =>
            {
                var dtoDoor = Doors.FirstOrDefault(d => d.DoorID == x.DoorID);
                dtoDoor.FloorDoorID = x.FloorDoorID;
                dtoDoor.FloorID = x.FloorID;
                dtoDoor.LocationX = x.LocationX;
                dtoDoor.LocationY = x.LocationY;
                dtoDoor.Rotation = x.Rotation;
                dtoDoor.Enabled = (x.FloorDoorID == 0 || x.FloorID == FloorID);
            });
        }


        private void Save()
        {
            string message = "";
            try
            {
                var coreModel = Mapper.Map<Floor>(this);
                if (FloorID == 0)
                {
                    coreModel = _floorRepo.Insert(coreModel);
                    FloorID = coreModel.FloorID;
                    message = "增加楼层成功!";
                }
                else
                {
                    coreModel = _floorRepo.Insert(coreModel);
                    _floorRepo.Update(coreModel);
                    message = "修改设楼层成功!";
                }

                ViewModelAttachment.CoreModel = coreModel;
                ViewModelAttachment.LastOperationSuccess = true;
            }
            catch (Exception ex)
            {
                Log.Error("Update floor fails.", ex);
                message = "保存楼层失败";
                SendMessage(message);
                return;
            }

            RaisePropertyChanged(null);
            Close(message);
        }

        private void Close(string message)
        {
            Messenger.Default.Send(new NotificationMessage(this, message), Tokens.FloorView_Close);
        }

        private void SendMessage(string message)
        {
            Messenger.Default.Send(new NotificationMessage(message), Tokens.FloorView_ShowNotification);
        }
    }
}
