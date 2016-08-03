using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Message.GetAllTimeSegmentsOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetAllTimeSegmentsResponse : ResponseBase
    {
        [DataMember]
        public IEnumerable<TimeSegmentService> Services { get; set; }

        public GetAllTimeSegmentsResponse()
        {
            Services = new List<TimeSegmentService>();
        }
    }
}
