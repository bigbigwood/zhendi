using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.Device
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DoorLinkageService : ServiceBase
    {
        [DataMember]
        public DoorLinkageOptions? OpenDoorOption { get; set; }
        [DataMember]
        public DoorLinkageOptions? AlarmOption { get; set; }
    }
}
