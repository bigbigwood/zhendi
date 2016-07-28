using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class BaseResponse
    {
        [DataMember]
        public ResultType ResultType { get; set; }
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
