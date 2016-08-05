using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceControlling
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DeviceService : ServiceBase
    {
        [DataMember]
        public Int32? AuthticationType { get; set; }
        [DataMember]
        public Int32? AutoOpenTimeZoneId { get; set; }
        [DataMember]
        public Boolean? AntiPassbackEnabled { get; set; }
        [DataMember]
        public IList<ServiceBase> Services { get; set; }

        public DeviceService()
        {
            Services = new List<ServiceBase>();
        }
    }
}
