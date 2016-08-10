using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceInfoResponse : ResponseBase
    {
        [DataMember]
        public DeviceInfo Service { get; set; }

        public GetDeviceInfoResponse()
        {
            Service = new DeviceInfo();
        }
    }
}
