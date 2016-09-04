using System;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UpdateDoorStateRequest : RequestBase
    {
        [DataMember]
        public Int32 DoorIndex { get; set; }
        [DataMember]
        public DoorControlOption Option { get; set; }
        
    }
}
