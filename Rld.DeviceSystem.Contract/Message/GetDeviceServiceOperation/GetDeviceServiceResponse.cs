using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services.DeviceControlling;

namespace Rld.DeviceSystem.Contract.Message.GetDeviceServiceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceServiceResponse : ResponseBase
    {
        [DataMember]
        public DeviceService Service { get; set; }

        public GetDeviceServiceResponse()
        {
            Service = new DeviceService();
        }
    }
}
