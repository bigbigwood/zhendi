using System;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceControlling
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DoorLinkageService : ServiceBase
    {
        [DataMember]
        public DoorLinkageAction? OpenDoorAction { get; set; }
        [DataMember]
        public DoorLinkageAction? AlarmAction { get; set; }
    }
}
