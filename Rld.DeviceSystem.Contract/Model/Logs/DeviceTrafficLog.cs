using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Model.Logs
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DeviceTrafficLog
    {
        [DataMember]
        public AccessLogType AccessLogType { get; set; }
        [DataMember]
        public Int32 DeviceId { get; set; }
        [DataMember]
        public Int32 UserId { get; set; }
        [DataMember]
        public IList<CheckInOptions> CheckInOptions { get; set; }
        [DataMember]
        public String Message { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Reserve for design purpose
        /// </summary>
        [DataMember]
        public Int32 DoorStatus { get; set; }
        /// <summary>
        /// Reserve for design purpose
        /// </summary>
        [DataMember]
        public Int32 Antipassback { get; set; }
        /// <summary>
        //// Reserve for design purpose
        /// </summary>
        [DataMember]
        public Int32 JobCode { get; set; }
    }
}
