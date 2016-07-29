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
    public class ResponseBase
    {
        [MessageBodyMember]
        [DataMember]
        public ResultType ResultType { get; set; }

        static IEnumerable<Type> KnownTypes()
        {
            IList<Type> typesOfResource =
                TypesResolver.GetKnownTypes(null).Where(x => typeof(ResponseBase).IsAssignableFrom(x)).ToList();
            return typesOfResource;
        }
    }

    [DataContract(Namespace = Declarations.NameSpace)]
    public enum ResultType
    {
        [EnumMember]
        OK = 0,
        [EnumMember]
        Error = 1,
    }
}
