using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Devices;

namespace Rld.DeviceSystem.Contract.Message.GetDeviceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceResponse : ResponseBase
    {
        [DataMember]
        public Device DeviceInfo { get; set; }

        public GetDeviceResponse()
        {
            DeviceInfo = new Device();
        }
    }
}
