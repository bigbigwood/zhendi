using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Model.Devices
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class Device
    {
        [DataMember]
        public Int32 DeviceId { get; set; }
        [DataMember]
        public String Mac { get; set; }
        [DataMember]
        public String SerialNumber { get; set; }
        [DataMember]
        public String Password { get; set; }
        [DataMember]
        public String DeviceModel { get; set; }
        [DataMember]
        public String CommunitionType { get; set; }
        [DataMember]
        public String Protocol { get; set; }
        [DataMember]
        public Boolean Status { get; set; }
        [DataMember]
        public String Remark { get; set; }
        [DataMember]
        public IEnumerable<ServiceBase> Services { get; set; }
        
        public Device()
        {
            Services = new List<ServiceBase>();
        }
    }
}
