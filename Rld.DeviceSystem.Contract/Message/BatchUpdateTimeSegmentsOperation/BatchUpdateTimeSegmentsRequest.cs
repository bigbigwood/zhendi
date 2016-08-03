using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Message.BatchUpdateTimeSegmentsOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class BatchUpdateTimeSegmentsRequest : RequestBase
    {
        [DataMember]
        public IEnumerable< TimeSegmentService> Services { get; set; }
    }
}
