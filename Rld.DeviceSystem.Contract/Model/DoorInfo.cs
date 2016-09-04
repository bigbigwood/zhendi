using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Model
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DoorInfo : ServiceBase
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public Int32 DoorIndex { get; set; }
        [DataMember]
        public DoorType DoorType { get; set; }
        [DataMember]
        public Int32? ElectricalAppliances { get; set; }
        [DataMember]
        public CheckOutOptions? CheckOutAction { get; set; }
        [DataMember]
        public Int32? AlertType { get; set; }
        [DataMember]
        public String Remark { get; set; }
        [DataMember]
        public IList<ServiceBase> Services { get; set; }
        public DoorInfo()
        {
            Services = new List<ServiceBase>();
        }
    }
}
