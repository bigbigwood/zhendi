using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rld.DeviceSystem.Contract.Message;

namespace Rld.DeviceSystem.Contract.Interface
{
    [ServiceContract(Namespace = Declarations.NameSpace)]
    public interface IUserService
    {
        [OperationContract]
        GetUserInfoResponse GetUser(GetUserInfoRequest request);
    }
}
