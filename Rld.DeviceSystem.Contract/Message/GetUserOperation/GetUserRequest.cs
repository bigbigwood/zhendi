using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Rld.DeviceSystem.Contract.Message.GetUserOperation
{
    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserRequest : RequestBase
    {
        [DataMember]
        public Int32 UserId { get; set; }
        //[DataMember]
        //public UserRequestType[] RequestTypes { get; set; }
    }
}
