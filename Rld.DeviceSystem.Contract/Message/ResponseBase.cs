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
        [DataMember(Order = 900)]
        public ResultType ResultType { get; set; }

        /// <summary>
        /// Help to identify the operation in the socket. Must be the same as the OperationGuid of the request
        /// </summary>
        [DataMember(Order = 901)]
        public String Token { get; set; }

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
        [EnumMember]
        NotSupport = 2,
    }
}
