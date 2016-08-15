using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.Model;
using Rld.DeviceSystem.Contract;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface IDeviceService
    {
        [OperationContract]
        SyncDeviceUserResponse SyncDeviceUser(SyncDeviceUserRequest request);

        [OperationContract]
        SyncDBUserResponse SyncDBUser(SyncDBUserRequest request);
    }
}
