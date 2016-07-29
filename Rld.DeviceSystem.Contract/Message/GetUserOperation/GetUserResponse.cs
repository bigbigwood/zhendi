using System.Runtime.Serialization;
using System.ServiceModel;
using Rld.DeviceSystem.Contract.Model.User;

namespace Rld.DeviceSystem.Contract.Message.GetUserOperation
{
    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserResponse : ResponseBase
    {
        [MessageBodyMember]
        [DataMember]
        public User UserInfo { get; set; }
    }
}
