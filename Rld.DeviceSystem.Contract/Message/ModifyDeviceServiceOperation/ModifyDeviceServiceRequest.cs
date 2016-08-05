using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.Device;

namespace Rld.DeviceSystem.Contract.Message.ModifyDeviceServiceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ModifyDeviceServiceRequest : RequestBase
    {
        [DataMember]
        public DeviceInfo Service { get; set; }
    }
}
