using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service
{
    public class InHouseUserService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceGroupRepository _deviceGroupRepo = NinjectBinder.GetRepository<IDeviceGroupRepository>();
        private IDeviceTrafficLogRepository _deviceTrafficLogRepo = NinjectBinder.GetRepository<IDeviceTrafficLogRepository>();

        public readonly List<string> IOMessages = new List<string>()
        {
            "Check in",
            "Clock in",
            "Clock out",
            "Customer in",
            "Customer out",
            "Out",
            "In",
            "User defined 1",
            "User defined 2",
        };

        public bool HasBindDeviceGroup(DeviceController device)
        {
            var deviceGroups = _deviceGroupRepo.Query(new Hashtable());
            var checkinDeviceIds = deviceGroups.Select(x => x.CheckInDeviceID);
            var checkoutDeviceIds = deviceGroups.Select(x => x.CheckOutDeviceID);

            return checkinDeviceIds.Contains(device.DeviceID) || checkoutDeviceIds.Contains(device.DeviceID);
        }

        public List<Int32> GetInHouseUsers(DeviceController device)
        {
            var deviceGroups = _deviceGroupRepo.Query(new Hashtable());
            var checkinDeviceIds = deviceGroups.Select(x => x.CheckInDeviceID);
            var checkoutDeviceIds = deviceGroups.Select(x => x.CheckOutDeviceID);

            DeviceController checkInDevice;
            DeviceController checkOutDevice;

            var deviceGroup = deviceGroups.FirstOrDefault(x => x.CheckInDeviceID == device.DeviceID);
            if (deviceGroup != null)
            {
                checkInDevice = device;
                checkOutDevice = ApplicationManager.GetInstance().AuthorizationDevices.FirstOrDefault(x => x.DeviceID == deviceGroup.CheckOutDeviceID);
            }
            else
            {
                deviceGroup = deviceGroups.FirstOrDefault(x => x.CheckOutDeviceID == device.DeviceID);
                checkInDevice = ApplicationManager.GetInstance().AuthorizationDevices.FirstOrDefault(x => x.DeviceID == deviceGroup.CheckInDeviceID);
                checkOutDevice = device;
            }

            var conditions = new Hashtable()
            { 
                { "DeviceId", 1 }, 
                { "StartDate", DateTime.Now.ToString("yyyy-MM-dd") }, 
                { "EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") },
            };

            conditions["DeviceId"] = checkInDevice.DeviceID;
            var checkinLogs = _deviceTrafficLogRepo.Query(conditions).FindAll(IsIOMessage);
            var checkinUserCodes = checkinLogs.Select(x => x.DeviceUserID);

            conditions["DeviceId"] = checkOutDevice.DeviceID;
            var checkoutLogs = _deviceTrafficLogRepo.Query(conditions).FindAll(IsIOMessage);
            var checkoutUserCodes = checkoutLogs.Select(x => x.DeviceUserID);

            var inHouseUsers = checkinUserCodes.Except(checkoutUserCodes);
            return inHouseUsers.ToList();
        }

        public bool IsIOMessage(DeviceTrafficLog deviceTrafficLogInfo)
        {
            return IOMessages.Any(x => x == deviceTrafficLogInfo.Remark);
        }
    }
}
