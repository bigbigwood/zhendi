using System;
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
    public class DeviceAdminLogMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(global::System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Contract.Model.Logs.DeviceOperationLog ToModel(Record record)
        {
            var logInfo = new Contract.Model.Logs.DeviceOperationLog();

            try
            {
                logInfo.AdminId = (int)record.MDIN;
                logInfo.UserId = (int) record.DIN;
                logInfo.Enroll = record.Verify.ToString();
                logInfo.Message = SLogType(record.Action);
                logInfo.CreateTime = record.Clock;

                return logInfo;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }


        public static string SLogType(int opertationType)
        {
            string message = string.Empty;

            switch (opertationType)
            {
                case 1:
                    message = "Registered user";
                    break;
                case 2:
                    message = "Register super administrator";
                    break;
                case 3:
                    message = "Register registrar";
                    break;
                case 4:
                    message = "Register query member";
                    break;
                case 5:
                    message = "Delete fingerprint";
                    break;
                case 6:
                    message = "Remove the password";
                    break;
                case 7:
                    message = "Delete the card number";
                    break;
                case 8:
                    message = "Remove access to records";
                    break;
                case 9:
                    message = "Remove records";
                    break;
                case 10:
                    message = "Set system information";
                    break;
                case 11:
                    message = "Setup time";
                    break;
                case 12:
                    message = "Set the record information";
                    break;
                case 13:
                    message = "Set the communications and information";
                    break;
                case 14:
                    message = "Set the access control information";
                    break;
                case 15:
                    message = "Set the user type";
                    break;
                case 16:
                    message = "Set the attendance time";
                    break;
            }
            return message;
        }


    }
}