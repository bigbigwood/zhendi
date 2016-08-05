using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.GetDeviceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceResponse : ResponseBase
    {
        [DataMember]
        public DeviceSystemInfo DeviceInfo { get; set; }

        public GetDeviceResponse()
        {
            DeviceInfo = new DeviceSystemInfo();
        }
    }
}
