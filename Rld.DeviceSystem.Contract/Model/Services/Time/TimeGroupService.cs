using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.Time
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class TimeGroupService : ServiceBase
    {
        [DataMember (Order = 1)]
        public Int32 TimeGroupId { get; set; }

        [DataMember (Order = 2)]
        public IEnumerable<Int32> TimeSegmentIds { get; set; }

        public TimeGroupService()
        {
            TimeSegmentIds = new List<Int32>();
        }
    }
}
