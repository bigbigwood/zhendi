using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceController
    {
        public Int32 DeviceID { get; set; }
        public String Mac { get; set; }
        public String DeviceCode { get; set; }
        public String SN { get; set; }
        public String Mode { get; set; }
        public CommunicationType CommunicationType { get; set; }
        public String BaudRate { get; set; }
        public String SerialPort { get; set; }
        public String Password { get; set; }
        public String IP { get; set; }
        public String Port { get; set; }
        public String Protocol { get; set; }
        public String Label { get; set; }
        public String ServerURL { get; set; }
        public String Remark { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DeviceControllerParameter DeviceControllerParameter { get; set; }
        public List<DeviceDoor> DeviceDoors { get; set; }
        public List<DeviceHeadReading> DeviceHeadReadings { get; set; }

        public DeviceController()
        {
            DeviceDoors= new List<DeviceDoor>();
            DeviceHeadReadings = new List<DeviceHeadReading>();
        }
    }
}
