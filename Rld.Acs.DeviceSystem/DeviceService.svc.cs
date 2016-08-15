using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.Model;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.Acs.DeviceSystem
{
    public class DeviceService : IDeviceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public  SyncDeviceUserResponse SyncDeviceUser(SyncDeviceUserRequest request)
        {


            var request2 = "request from helloworld";
            var operation = new WebSocketOperation();
            var response = operation.Execute(request2);

            Log.Info("return helloworld");

            throw new NotImplementedException();
        }

        public SyncDBUserResponse SyncDBUser(SyncDBUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
