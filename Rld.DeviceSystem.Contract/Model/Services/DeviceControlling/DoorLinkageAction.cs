using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceControlling
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum DoorLinkageAction
    {
        [EnumMember] 
        None = 0,
        [EnumMember]
        Master = 1,
        [EnumMember]
        Slave = 2,
        [EnumMember]
        All = 3,
    }
}
