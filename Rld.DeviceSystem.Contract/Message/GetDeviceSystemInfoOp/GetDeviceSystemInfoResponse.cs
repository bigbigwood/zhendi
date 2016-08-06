using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.GetDeviceSystemInfoOp
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceSystemInfoResponse : ResponseBase
    {
        [DataMember]
        public DeviceSystemInfo DeviceInfo { get; set; }

        public GetDeviceSystemInfoResponse()
        {
            DeviceInfo = new DeviceSystemInfo();
        }
    }
}
