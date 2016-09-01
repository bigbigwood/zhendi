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
        public ObservableCollection<FloorDoorViewModel> Doors { get; set; }
        public virtual Int32 FloorID { get; set; }
        public String Name { get; set; }
        public String Photo { get; set; }
        public GeneralStatus Status { get; set; }


        public FloorViewModel()
        {
            SaveCmd = new RelayCommand(Save);
            CancelCmd = new RelayCommand(() => Close(""));

            Doors = new ObservableCollection<FloorDoorViewModel>();
            ApplicationManager.GetInstance().AuthorizationDoors.ForEach(x => Doors.Add(new FloorDoorViewModel()
            {
                FloorDoorID = 0,
                DoorID = x.DeviceDoorID,
                DoorName = x.Name,
            }));
        }

        public List<FloorDoor> GetUIDoors()
        {
            var coreDoors = new List<FloorDoor>();
            Doors.FindAll(x => x.FloorID != 0).ForEach(x => coreDoors.Add(new FloorDoor()
            {
                FloorDoorID = x.FloorDoorID,
                DoorID = x.DoorID,
                FloorID = x.FloorID,
                DoorType = x.DoorType,
                LocationX = x.LocationX,
                LocationY = x.LocationY,
                Rotation = x.Rotation,
            }));

            return coreDoors;
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
            });
        }

        private void UploadImage()
        {
            try
            {
                if (!File.Exists(Photo))
                    throw new Exception("file does not exist.");

                string extension = new FileInfo(Photo).Extension;

                string uniqueFileName = string.Format(@"{0}_{1}{2}", Guid.NewGuid(), DateTime.Now.ToString("yyyyMMddhhmmss"), extension);
                string cacheFilePath = string.Format(@"{0}\{1}", ApplicationManager.GetInstance().LocalImageCachePath, uniqueFileName);
                File.Copy(Photo, cacheFilePath);

                //_userAvatorService.UploadAvatorToServer(uniqueFileName);
                //Avator = cacheFilePath;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        private void Save()
        {
            string message = "";
            try
            {
                if (FloorID == 0)
                {
                    var coreModel = Mapper.Map<Floor>(this);
                    coreModel = _floorRepo.Insert(coreModel);
                    FloorID = coreModel.FloorID;
                    message = "增加楼层成功!";
                }
                else
                {
                    var coreModel = Mapper.Map<Floor>(this);
                    coreModel = _floorRepo.Insert(coreModel);
                    _floorRepo.Update(coreModel);
                    message = "修改设楼层成功!";
                }
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
