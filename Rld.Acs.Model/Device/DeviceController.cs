using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceController
    {
        public virtual Int32 DeviceID { get; set; }
        public virtual String Mac { get; set; }
        public virtual String DeviceCode { get; set; }
        public virtual String SN { get; set; }
        public virtual String Mode { get; set; }
        public virtual CommunicationType CommunicationType { get; set; }
        public virtual String BaudRate { get; set; }
        public virtual String SerialPort { get; set; }
        public virtual String Password { get; set; }
        public virtual String IP { get; set; }
        public virtual String Port { get; set; }
        public virtual String Protocol { get; set; }
        public virtual String Label { get; set; }
        public virtual String ServerURL { get; set; }
        public virtual String Remark { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual DeviceControllerParameter DeviceControllerParameter { get; set; }
        public virtual List<DeviceDoor> DeviceDoors { get; set; }
        public virtual List<DeviceHeadReading> DeviceHeadReadings { get; set; }

        public DeviceController()
        {
            DeviceDoors= new List<DeviceDoor>();
            DeviceHeadReadings = new List<DeviceHeadReading>();
        }
    }
}
