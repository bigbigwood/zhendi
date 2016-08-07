using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Rld.DeviceSystem.Contract.Message.GetUserInfoOp
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserInfoRequest : RequestBase
    {
        [DataMember]
        public Int32 UserId { get; set; }
    }
}
