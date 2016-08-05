using System;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceControlling
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DoorService : ServiceBase
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public Int32? ElectricalAppliances { get; set; }
        [DataMember]
        public Int32? OpenType { get; set; }
        [DataMember]
        public String Remark { get; set; }
        [DataMember]
        public Int32? AlertType { get; set; }
        [DataMember]
        public Int32 OpenDelaySeconds { get; set; }
        [DataMember]
        public AlarmService AlarmService { get; set; }
        [DataMember]
        public DuressService DuressService { get; set; }

        public DoorService()
        {
            AlarmService = new AlarmService();
        }
    }
}
