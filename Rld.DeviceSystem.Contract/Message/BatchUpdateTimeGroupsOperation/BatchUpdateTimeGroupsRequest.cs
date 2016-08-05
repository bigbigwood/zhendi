using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;
using Rld.DeviceSystem.Contract.Model.Services.Time;

namespace Rld.DeviceSystem.Contract.Message.BatchUpdateTimeGroupsOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class BatchUpdateTimeGroupsRequest : RequestBase
    {
        [DataMember]
        public IEnumerable<TimeGroupService> Services { get; set; }
    }
}
