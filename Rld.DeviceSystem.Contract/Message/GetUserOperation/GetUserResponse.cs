using System.Runtime.Serialization;
using System.ServiceModel;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.GetUserOperation
{
    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserResponse : ResponseBase
    {
        [MessageBodyMember]
        [DataMember]
        public UserInfo UserInfo { get; set; }

        public GetUserResponse()
        {
            UserInfo = new UserInfo();
        }
    }
}
