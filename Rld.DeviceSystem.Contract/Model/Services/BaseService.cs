using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class BaseService
    {
        [DataMember]
        public Boolean Enabled { get; set; }
        [DataMember]
        public KeyValue[] ExtendData { get; set; }
    }
}
