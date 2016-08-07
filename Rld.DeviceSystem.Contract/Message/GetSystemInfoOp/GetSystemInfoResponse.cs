using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.GetSystemInfoOp
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetSystemInfoResponse : ResponseBase
    {
        [DataMember]
        public SystemInfo SystemInfo { get; set; }

        public GetSystemInfoResponse()
        {
            SystemInfo = new SystemInfo();
        }
    }
}
