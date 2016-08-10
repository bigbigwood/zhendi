using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetUserInfoRequest : RequestBase
    {
        [DataMember]
        public Int32 UserId { get; set; }
    }
}
