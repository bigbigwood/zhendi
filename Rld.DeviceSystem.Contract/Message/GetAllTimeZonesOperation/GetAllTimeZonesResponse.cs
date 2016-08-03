using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Message.GetAllTimeZonesOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetAllTimeZonesResponse : ResponseBase
    {
        [DataMember]
        public IEnumerable<TimeZoneService> Services { get; set; }

        public GetAllTimeZonesResponse()
        {
            Services = new List<TimeZoneService>();
        }
    }
}
