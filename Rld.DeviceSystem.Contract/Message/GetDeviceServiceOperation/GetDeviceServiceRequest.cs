using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Message.GetDeviceServiceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceServiceRequest : RequestBase
    {
        [DataMember]
        public Int32 DeviceId { get; set; }
    }
}
