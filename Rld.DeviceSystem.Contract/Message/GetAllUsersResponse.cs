using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetAllUsersResponse : ResponseBase
    {
        [DataMember]
        public IList<UserInfo> Users { get; set; }

        public GetAllUsersResponse()
        {
            Users = new List<UserInfo>();
        }
    }
}
