using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Rld.Acs.Model;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem.Message
{
    [MessageContract(IsWrapped = true)]
    public class SyncDepartmentUsersRequest : RequestBase
    {
        [MessageBodyMember]
        public List<Department> Departments { get; set; }

        [MessageBodyMember]
        public List<DeviceController> Devices { get; set; }
    }

    [MessageContract(IsWrapped = true)]
    public class SyncDepartmentUsersResponse : ResponseBase
    {
    }
}