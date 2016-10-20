using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Rld.DeviceSystem.Contract;

namespace Rld.Acs.DeviceSystem.Model
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DeviceUserDto
    {
        [DataMember]
        public Int32 UserCode { get; set; }
        [DataMember]
        public String UserName { get; set; }
    }
}