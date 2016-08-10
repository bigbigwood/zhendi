using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UpdateSystemInfoRequest : RequestBase
    {
        [DataMember]
        public SystemInfo SystemInfo { get; set; }
    }
}
