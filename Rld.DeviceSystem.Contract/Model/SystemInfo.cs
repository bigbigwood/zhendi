using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Model
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class SystemInfo
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
        public IList<ServiceBase> Services { get; set; }
        
        public SystemInfo()
        {
            Services = new List<ServiceBase>();
        }
    }
}
