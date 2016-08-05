using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceConnection
{
    [DataContract(Namespace = Declarations.NameSpace)]
    [KnownType("KnownTypes")]
    public class DeviceConnectionService : ServiceBase
    {
        static IEnumerable<Type> KnownTypes()
        {
            IList<Type> typesOfResource =
                TypesResolver.GetKnownTypes(null).Where(x => typeof(DeviceConnectionService).IsAssignableFrom(x)).ToList();
            return typesOfResource;
        }
    }
}
