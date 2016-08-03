using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services;

namespace Rld.DeviceSystem.Contract.Message.UpdateTimeSegmentOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UpdateTimeSegmentRequest : RequestBase
    {
        [DataMember]
        public TimeSegmentService Service { get; set; }
    }
}
