using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Model.Logs
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DeviceOperationLog
    {
        [DataMember]
        public Int32 AdminId { get; set; }
        [DataMember]
        public Int32 UserId { get; set; }
        [DataMember]
        public Int32 OperationType { get; set; }
        [DataMember]
        public String Message { get; set; }
        [DataMember]
        public String Enroll { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
    }
}
