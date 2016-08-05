using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;
using Rld.DeviceSystem.Contract.Model.Services.Time;

namespace Rld.DeviceSystem.Contract.Message.BatchUpdateTimeZonesOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class BatchUpdateTimeZonesRequest : RequestBase
    {
        [DataMember]
        public IEnumerable<TimeZoneService> Services { get; set; }
    }
}
