using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services.DeviceControlling;

namespace Rld.DeviceSystem.Contract.Message.ModifyDeviceServiceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ModifyDeviceServiceRequest : RequestBase
    {
        [DataMember]
        public DeviceService Service { get; set; }
    }
}
