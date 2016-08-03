using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Message.GetAllTimeGroupsOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetAllTimeGroupsResponse : ResponseBase
    {
        [DataMember]
        public IEnumerable<TimeGroupService> Services { get; set; }

        public GetAllTimeGroupsResponse()
        {
            Services = new List<TimeGroupService>();
        }
    }
}
