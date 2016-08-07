using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Dao
{
    public class SystemInfoDao
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;

        public SystemInfoDao(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public SystemEntity GetSystemData()
        {
            var entity = new SystemEntity();
            var device = _deviceProxy.Device;
            bool result;
            object extraProperty = new object();
            object extraData = new object();

            try
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                entity.DeviceId = GetSystemParameter(SystemParameters.DeviceID);
                entity.BaudRate = GetSystemParameter(SystemParameters.Baudrate);
                entity.Password = GetSystemParameter(SystemParameters.DevicePassword);
                entity.IP = GetSystemParameter(SystemParameters.IPAddress);
                entity.Port = GetSystemParameter(SystemParameters.UDPPort);
                entity.ServerURL = GetSystemParameter(SystemParameters.ServerIPAddress);
                entity.ServerPort = GetSystemParameter(SystemParameters.ServerUDPPort);


                extraData = Zd2911Utils.DeviceModel;
                result = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.Model, extraProperty, ref device, ref extraData);
                entity.Model = (string)extraData;

                extraData = Zd2911Utils.DeviceSerialNo;
                result = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.SerialNo, extraProperty, ref device, ref extraData);
                entity.SerialNumber = (string)extraData;

                result = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.MacAddress, extraProperty, ref device, ref extraData);
                if (result)
                {
                    var bytes = (byte[])extraData; //bytes.Length = 6 } 
                    entity.Mac = ConvertObject.ConvertByteToHex(bytes);
                }

                return entity;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
            finally
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceIdle);
            }
        }

        public bool UpdateSystemData(SystemEntity entity)
        {
            var device = _deviceProxy.Device;
            bool result = true;
            object extraProperty = new object();
            object extraData = new object();

            try
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                SetSystemParameter(SystemParameters.DeviceID, entity.DeviceId);
                SetSystemParameter(SystemParameters.Baudrate, entity.BaudRate);
                SetSystemParameter(SystemParameters.DevicePassword, entity.Password);
                SetSystemParameter(SystemParameters.IPAddress, entity.IP);
                SetSystemParameter(SystemParameters.UDPPort, entity.Port);
                SetSystemParameter(SystemParameters.ServerIPAddress, entity.ServerURL);
                SetSystemParameter(SystemParameters.ServerUDPPort, entity.ServerPort);

                extraProperty = Zd2911Utils.DeviceModel;
                extraData = entity.Model;
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Model, extraProperty, device, extraData);

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
            finally
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceIdle);
            }
        }

        private String GetSystemParameter(SystemParameters parameter)
        {
            Log.InfoFormat("Getting system parameter: {0}", parameter.ToString());
            var systemItemValue ="";
            var extraProperty = new object();
            var extraData = new object();
            var device = _deviceProxy.Device;

            var index = (int) parameter;
            byte[] paramIndex = BitConverter.GetBytes(index);
            var retryablePolicy = Policies.GetRetryablePolicy();
            bool result = retryablePolicy.Execute(
                () =>
                {
                    extraData = paramIndex;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.SysParam, extraProperty, ref device, ref extraData);
                });

            if (result)
            {
                int paramValue = BitConverter.ToInt32((byte[])extraData, 0);
                switch (index)
                {
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                        systemItemValue = ConvertObject.ConvertNumberToIPAddress(paramValue);
                        break;
                    default:
                        systemItemValue = paramValue.ToString();
                        break;
                }
            }

            Log.InfoFormat("Getting system parameter Result={0}, value={1}", result, systemItemValue);
            return systemItemValue;
        }

        private bool SetSystemParameter(SystemParameters parameter, String parameterValue)
        {
            Log.InfoFormat("Setting system parameter: {0}", parameter.ToString());
            if (string.IsNullOrWhiteSpace(parameterValue))
            {
                Log.Info("parameter value is empty. skip the process");
                return true;
            }

            int parameterIndex = (Int32) parameter;
            int paramValue = -1;
            switch (parameterIndex)
            {
                case 26:
                case 27:
                case 28:
                case 29:
                    paramValue = ConvertObject.ConvertIPAddressToNumber(parameterValue);
                    break;
                default:
                    paramValue = Convert.ToInt32(parameterValue);
                    break;
            }

            var device = _deviceProxy.Device;
            object extraProperty = new object();
            object extraData = new object();
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes(parameterIndex), 0, data, 0, 4);
            Array.Copy(BitConverter.GetBytes(paramValue), 0, data, 4, 4);
            extraData = data;

            var retryablePolicy = Policies.GetRetryablePolicy();
            bool result = retryablePolicy.Execute(
                () => _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.SysParam, extraProperty, device, extraData));

            Log.InfoFormat("Setting system parameter Result={0}", result);
            return result;
        }
    }
}