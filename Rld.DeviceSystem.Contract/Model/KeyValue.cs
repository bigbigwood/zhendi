using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class KeyValue
    {
        [DataMember]
        public String Key { get; set; }
        [DataMember]
        public String Value { get; set; }
    }
}
