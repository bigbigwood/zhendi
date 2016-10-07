using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Rld.DeviceSystem.Contract;

namespace Rld.Acs.DeviceSystem.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum SyncOption
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        Create = 1,
        [EnumMember]
        Update = 2,
        [EnumMember]
        Delete = 3,
    }
}