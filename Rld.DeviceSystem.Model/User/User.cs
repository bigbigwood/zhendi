using System;
using Rld.DeviceSystem.Model.Services;

namespace Rld.DeviceSystem.Model.User
{
    public class User
    {
        public Int32 UserId { get; set; }
        public String ExternalUserCode { get; set; }
        public UserRole? Role { get; set; }
        public String UserName { get; set; }
        public Boolean UserStatus { get; set; }
        public Int32 DepartmentId { get; set; }
        public String Comment { get; set; }
        public CredentialService[] CredentialServices { get; set; }
    }
}
