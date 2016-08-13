using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.DeviceConn;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.SystemInfo;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Log
{
    public class DeviceAccessLogMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(global::System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly List<Int32> AlarmOptions = new List<int>() { 19, 20, 21 }; //19=开门超时报警, 20=强行开门报警 21=胁迫报警
        private static readonly List<Int32> ExceptionOptions = new List<int>() { 26, 27, 31 }; //26=无效时间, 27=无效通行 31=无效操作

        public static Contract.Model.Logs.DeviceAccessLog ToModel(Record record)
        {
            var logInfo = new Contract.Model.Logs.DeviceAccessLog();

            try
            {
                logInfo.DeviceId = record.DN;
                logInfo.UserId = (int) record.DIN;
                logInfo.AccessLogType = GetAccessLogType(record.Verify);
                logInfo.CheckInOptions = GLogType(record.Action);
                logInfo.Message = IOMode(record.Verify);
                logInfo.CreateTime = record.Clock;

                return logInfo;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static AccessLogType GetAccessLogType(int mode)
        {
            if (AlarmOptions.Contains(mode))
                return AccessLogType.Alarm;
            if (ExceptionOptions.Contains(mode))
                return AccessLogType.Exception;
            return AccessLogType.General;
        }

        public static string IOMode(int mode)
        {
            string message = string.Empty;

            switch (mode)
            {
                case 0:
                    message = "Check in";
                    break;
                case 1:
                    message = "Clock in";
                    break;
                case 2:
                    message = "Clock out";
                    break;
                case 3:
                    message = "Customer in";
                    break;
                case 4:
                    message = "Customer out";
                    break;
                case 5:
                    message = "Out";
                    break;
                case 6:
                    message = "In";
                    break;
                case 7:
                    message = "User defined 1";
                    break;
                case 8:
                    message = "User defined 2";
                    break;
                case 12:
                    message = "Button open";
                    break;
                case 13:
                    message = "Software open";
                    break;
                case 14:
                    message = "Keep open";
                    break;
                case 15:
                    message = "Keep close";
                    break;
                case 16:
                    message = "Auto mode";
                    break;
                case 17:
                    message = "Open in";
                    break;
                case 18:
                    message = "Open out";
                    break;
                case 19:
                    message = "Overtime open alarm";
                    break;
                case 20:
                    message = "Forced open alarm";
                    break;
                case 21:
                    message = "Antihijack alarm";
                    break;
                case 22:
                    message = "Input action 1";
                    break;
                case 23:
                    message = "Input action 2";
                    break;
                case 24:
                    message = "Output action 1";
                    break;
                case 25:
                    message = "Output action 2";
                    break;
                case 26:
                    message = "Invalid time";
                    break;
                case 27:
                    message = "Invalid date";
                    break;
                case 31:
                    message = "Illegal operation";
                    break;
            }
            return message;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="opertationType"></param>
        /// <returns></returns>
        public static List<CheckInOptions> GLogType(int opertationType)
        {
            var options = new List<CheckInOptions>();
            for (int i = 0; i < 4; i++)
            {
                if (0 != Zd2911Utils.BitCheck(opertationType, i))
                {
                    switch (i)
                    {
                        case 0:
                            options.Add(CheckInOptions.FingerPrint);
                            break;
                        case 1:
                            options.Add(CheckInOptions.Password);
                            break;
                        case 2:
                            options.Add(CheckInOptions.Card);
                            break;
                        case 3:
                            options.Add(CheckInOptions.Wiegand);
                            break;
                    }
                }
            }
            return options;
        }
    }
}