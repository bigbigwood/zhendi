using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Model.Services
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
