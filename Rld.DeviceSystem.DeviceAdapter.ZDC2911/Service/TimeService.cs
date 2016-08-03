using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model.Services;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service
{
    public class TimeService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;
        public TimeService(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public IEnumerable<Contract.Model.Services.TimeSegmentService> GetAllTimeSegmentServices()
        {
            var timesegments = new List<TimeSegmentService>();
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            try
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(()
                    =>
                {
                    extraData = Zd2911Utils.DevicePassSegment;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassSegment, extraProperty,
                        ref device, ref extraData);
                });

                if (result)
                {
                    
                    byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                    for (int index = 0; index < Zd2911Utils.PassItemCount; index++)
                    {
                        var timeSegmentService = new TimeSegmentService()
                        {
                            TimeSegmentId = index,
                            StartHour = Convert.ToInt32(data[index * 4]),
                            StartMinute = Convert.ToInt32(data[index * 4 + 1]),
                            EndHour = Convert.ToInt32(data[index * 4 + 2]),
                            EndMinute = Convert.ToInt32(data[index * 4 + 3]),
                        };

                        timesegments.Add(timeSegmentService);
                    }
                }

                return timesegments;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool BatchUpdateTimeSegmentServices(IEnumerable<Contract.Model.Services.TimeSegmentService> services)
        {


            bool result = false;
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();

            try
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                result = retryablePolicy.Execute(()
                    =>
                {
                    extraData = Zd2911Utils.DevicePassSegment;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassSegment, extraProperty,
                        ref device, ref extraData);
                });
                if (result == false)
                {
                    throw new Exception("cannot get timesegments");
                }

                byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                foreach (var timeSegmentService in services)
                {
                    Int32 index = timeSegmentService.TimeSegmentId * 4;
                    data[index] = (Byte)timeSegmentService.StartHour;
                    data[index + 1] = (Byte)timeSegmentService.StartMinute;
                    data[index + 2] = (Byte)timeSegmentService.EndHour;
                    data[index + 3] = (Byte)timeSegmentService.EndMinute;
                }

                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                extraData = Encoding.Unicode.GetString(data);
                extraProperty = Zd2911Utils.DevicePassSegment;
                //var retryablePolicy = Policies.GetRetryablePolicy();
                result = retryablePolicy.Execute(()
                    => _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.PassSegment, extraProperty, device, extraData));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            finally
            {
                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceIdle);
            }

            return result;
        }
    }
}