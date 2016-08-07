using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    [KnownType("KnownTypes")]
    public class RequestBase
    {

        static IEnumerable<Type> KnownTypes()
        {
            IList<Type> typesOfResource =
                TypesResolver.GetKnownTypes(null).Where(x => typeof(RequestBase).IsAssignableFrom(x)).ToList();
            return typesOfResource;
        }
    }
}
