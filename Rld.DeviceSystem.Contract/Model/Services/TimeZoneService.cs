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
        public TimeGroupService MondayTimeGroup { get; set; }
        [DataMember(Order = 3)]
        public TimeGroupService TuesdayTimeGroup { get; set; }
        [DataMember(Order = 4)]
        public TimeGroupService WednesdayTimeGroup { get; set; }
        [DataMember(Order = 5)]
        public TimeGroupService ThursdayTimeGroup { get; set; }
        [DataMember(Order = 6)]
        public TimeGroupService FridayTimeGroup { get; set; }
        [DataMember(Order = 7)]
        public TimeGroupService SaturdayTimeGroup { get; set; }
        [DataMember(Order = 8)]
        public TimeGroupService SundayTimeGroup { get; set; }
    }
}
