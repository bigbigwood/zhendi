using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Message.GetTimeSegmentOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetTimeSegmentRequest : RequestBase
    {
        [DataMember]
        public Int32 Id { get; set; }
    }
}
