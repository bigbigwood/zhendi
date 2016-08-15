using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.DeviceSystem.Service;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem
{
    public class DeviceService : IDeviceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SyncDeviceUserResponse SyncDeviceUser(SyncDeviceUserRequest request)
        {
            PersistenceOperation.Process(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserRepository>();
                var userInfo = repo.GetByKey(3);

                new UserOperation().UpdateDeviceUser(userInfo);
            });

            return new SyncDeviceUserResponse() { ResultType = ResultTypes.Ok };
        }

        public SyncDBUserResponse SyncDBUser(SyncDBUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
