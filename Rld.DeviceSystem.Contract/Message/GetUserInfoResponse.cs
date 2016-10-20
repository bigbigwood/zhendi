using System.Runtime.Serialization;
using System.ServiceModel;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserInfoResponse : ResponseBase
    {
        [DataMember]
        public UserInfo UserInfo { get; set; }

        public GetUserInfoResponse()
        {
            UserInfo = new UserInfo();
        }
    }
}
