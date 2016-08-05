using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;

namespace Rld.DeviceSystem.Contract.Message.GetDeviceServiceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceServiceResponse : ResponseBase
    {
        [DataMember]
        public DeviceInfo Service { get; set; }

        public GetDeviceServiceResponse()
        {
            Service = new DeviceInfo();
        }
    }
}
