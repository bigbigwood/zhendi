using System.Security.Principal;
using Rld.Acs.Model;


namespace Rld.Acs.WebApi.Framework
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {
            this.Password = password;
        }

        /// <summary>
        /// Basic Auth Password for custom authentication
        /// </summary>
        public string Password { get; set; }

        public SysOperator AuthorizationOperatorInfo { get; set; }
    }
}