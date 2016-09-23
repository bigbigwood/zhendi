using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;
using Rld.DeviceSystem.Contract;

namespace Rld.Acs.DeviceSystem.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public enum ResultTypes
    {
        [EnumMember]
        Ok = 0,
        [EnumMember]
        AuthenticationError = 1,
        [EnumMember]
        AuthorizationError = 2,
        [EnumMember]
        DataValidationError = 3,
        [EnumMember]
        BussinessLogicError = 4,
        [EnumMember]
        NotSupportError = 5,
        [EnumMember]
        Queued = 98,
        [EnumMember]
        UnknownError = 99,
    }

    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class RequestBase
    {
    }

    [MessageContract(IsWrapped = true)]
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ResponseBase
    {
        [DataMember(Order = 900)]
        [MessageBodyMember(Order = 900)]
        public ResultTypes ResultType { get; set; }

        //[DataMember(Order = 950)]
        //[MessageBodyMember(Order = 950)]
        //public String JsonResult { get; set; }

        [DataMember(Order = 999)]
        [MessageBodyMember(Order = 999)]
        public String[] Messages { get; set; }
    }
}