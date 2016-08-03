using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Model.Services
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class TimeZoneService : ServiceBase
    {
        [DataMember(Order = 1)]
        public Int32 TimeZoneId { get; set; }
        [DataMember(Order = 2)]
        public Int32? MondayTimeGroupId { get; set; }
        [DataMember(Order = 3)]
        public Int32? TuesdayTimeGroupId { get; set; }
        [DataMember(Order = 4)]
        public Int32? WednesdayTimeGroupId { get; set; }
        [DataMember(Order = 5)]
        public Int32? ThursdayTimeGroupId { get; set; }
        [DataMember(Order = 6)]
        public Int32? FridayTimeGroupId { get; set; }
        [DataMember(Order = 7)]
        public Int32? SaturdayTimeGroupId { get; set; }
        [DataMember(Order = 8)]
        public Int32? SundayTimeGroupId { get; set; }
    }
}
