using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Message.GetDeviceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceRequest : RequestBase
    {
        [DataMember]
        public Int32 DeviceId { get; set; }
    }
}
