using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Model.Logs
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DoorStateInfo
    {
        [DataMember]
        public Int32 DoorIndex { get; set; }

        /// <summary>
        /// 1 = opened, 0 = closed
        /// </summary>
        [DataMember]
        public Boolean Opened { get; set; }
    }
}
