using System.Text;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model.Services.Device;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.Device
{
    public class DuressServiceMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static DuressService BuildService(byte[] deviceData)
        {
            var duressService = new DuressService();
            duressService.Enabled = (deviceData[6] == 1);
            duressService.FingerPrintIndex = deviceData[5];
            duressService.IsOpenDoor = (deviceData[1] == 1);
            duressService.IsTriggerAlarm = (deviceData[2] == 1);
            var pwd = new StringBuilder();
            for (int i = 0; i < Zd2911Utils.PasswordLength; i++)
            {
                pwd.Append((char)deviceData[8 + i]);
            }
            duressService.Password = pwd.ToString();

            return duressService;
        }

        public static void UpdateDeviceData(ref byte[] deviceData, DuressService duressService)
        {
            deviceData[6] = (byte)(duressService.Enabled ? 1 : 0);

            if (duressService.FingerPrintIndex.HasValue)
                deviceData[5] = (byte)duressService.FingerPrintIndex;

            if (duressService.IsOpenDoor.HasValue)
                deviceData[1] = (byte)(duressService.IsOpenDoor.Value ? 1 : 0);

            if (duressService.IsTriggerAlarm.HasValue)
                deviceData[2] = (byte)(duressService.IsTriggerAlarm.Value ? 1 : 0);

            string pwd = duressService.Password;
            for (int i = 0; i < Zd2911Utils.PasswordLength; i++)
            {
                if (i < (pwd.Length))
                    deviceData[8 + i] = (byte)pwd[i];
                else
                    deviceData[8 + i] = 0;
            }
        }
    }
}