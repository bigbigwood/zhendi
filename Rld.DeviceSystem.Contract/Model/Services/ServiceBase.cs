using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services
{
    [DataContract(Namespace = Declarations.NameSpace)]
    [KnownType("KnownTypes")]
    public class ServiceBase
    {
        [DataMember(Order = 900)]
        public Boolean Enabled { get; set; }

        [DataMember(Order = 901)]
        public KeyValue[] ExtendData { get; set; }

        public ServiceBase()
        {
            Enabled = true;
        }

        static IEnumerable<Type> KnownTypes()
        {
            IList<Type> typesOfResource =
                TypesResolver.GetKnownTypes(null).Where(x => typeof(ServiceBase).IsAssignableFrom(x)).ToList();
            return typesOfResource;
        }
    }
}
