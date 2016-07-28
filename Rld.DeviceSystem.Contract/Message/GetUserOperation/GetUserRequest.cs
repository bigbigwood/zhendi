using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Message.GetUserOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserRequest : BaseRequest
    {
        [DataMember]
        public Int32 UserId { get; set; }
        [DataMember]
        public UserRequestType[] RequestTypes { get; set; }
    }
}
