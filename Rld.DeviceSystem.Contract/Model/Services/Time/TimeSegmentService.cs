using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.Time
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class TimeSegmentService : ServiceBase
    {
        [DataMember(Order = 1)]
        public Int32 TimeSegmentId { get; set; }
        [DataMember(Order = 2)]
        public Int32 StartHour { get; set; }
        [DataMember(Order = 3)]
        public Int32 StartMinute { get; set; }
        [DataMember(Order = 4)]
        public Int32 EndHour { get; set; }
        [DataMember(Order = 5)]
        public Int32 EndMinute { get; set; }
    }
}
