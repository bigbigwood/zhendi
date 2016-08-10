using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao
{
    public class GLogInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;

        public GLogInfoDao(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public List<Record> GetLogData(QueryLogCondictions conditions)
        {
            var logs = new List<Record>();
            
            bool result;
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();

            var datetimeRange = new List<DateTime>()
            {
                conditions.BeginTime.Value, 
                conditions.EndTime.Value,
            }; ;

            var boolList = new List<bool>()
            {
                conditions.Options == GetLogOptions.GetNewLogs, //新日志，该参数为true时，第二个参数清除新日志标记值为true或false时都将强制清除新日志标记
                conditions.CleanNewLogPosition,//清除新日志标记
            };

            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                extraProperty = (conditions.Options == GetLogOptions.GetNewLogs);
                extraData = datetimeRange;
                result = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.AttRecordsCount, extraProperty, ref device, ref extraData);

                int recordCount;
                int.TryParse(extraData.ToString(), out recordCount);
                if (result == false || recordCount == 0)
                {
                    return logs;
                }

                extraProperty = boolList;
                extraData = datetimeRange;
                result = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.AttRecords, extraProperty, ref device, ref extraData);
                if (result)
                {
                    logs = (List<Record>)extraData;
                }

                return logs;
            }
        }

        public bool DeleteAllLog()
        {
            bool result;
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            using (var operation = new DeviceLockableOperation(_deviceProxy))
            {
                result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.AttRecords, extraProperty, device, extraData);
                return result;
            }
        }
    }
}