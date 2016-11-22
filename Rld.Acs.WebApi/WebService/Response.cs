using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.Acs.Model;

namespace Rld.Acs.WebApi.WebService
{
    public enum ResultType
    {
        OK = 0,
        UserNotFound = 1,
        AuthenticationError = 2,
        AuthorizationError = 3,
        UnknownError = 4,
    }
    [Serializable]
    public class GenericResult
    {
        public ResultType ResultType { get; set; }
        public string[] Messages { get; set; }
    }
    [Serializable]
    public class AuthenticateResult : GenericResult
    {
        public SysOperator OperatorInfo { get; set; }
    }
}