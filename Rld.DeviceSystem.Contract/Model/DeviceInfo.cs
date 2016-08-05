using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Model
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DeviceInfo : ServiceBase
    {
        [DataMember]
        public Int32? AuthticationType { get; set; }
        [DataMember]
        public Int32? AutoOpenTimeZoneId { get; set; }
        [DataMember]
        public Boolean? AntiPassbackEnabled { get; set; }
        [DataMember]
        public IList<ServiceBase> Services { get; set; }

        public DeviceInfo()
        {
            Services = new List<ServiceBase>();
        }
    }
}
