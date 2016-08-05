using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;
using Rld.DeviceSystem.Contract.Model.Services.Time;

namespace Rld.DeviceSystem.Contract.Message.GetAllTimeGroupsOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetAllTimeGroupsResponse : ResponseBase
    {
        [DataMember]
        public IList<TimeGroupService> Services { get; set; }

        public GetAllTimeGroupsResponse()
        {
            Services = new List<TimeGroupService>();
        }
    }
}
