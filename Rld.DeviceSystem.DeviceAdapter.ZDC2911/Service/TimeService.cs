using System;
using System.Collections.Generic;
using System.Data.Common;
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
        private static readonly List<Int32> ZeroToNineCollection = new List<Int32> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private DeviceProxy _deviceProxy = null;
        public TimeService(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        #region Time Segment
        public IEnumerable<TimeSegmentService> GetAllTimeSegmentServices()
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

        public bool BatchUpdateTimeSegmentServices(IEnumerable<TimeSegmentService> services)
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
                extraData = Encoding.Unicode.GetString(data);

                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                result = retryablePolicy.Execute(
                   () => _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.PassSegment, Zd2911Utils.DevicePassSegment, device, extraData));
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
        #endregion

        #region TimeGroup
        public IEnumerable<TimeGroupService> GetAllTimeGroupServices()
        {
            var timeGroupServices = new List<TimeGroupService>();
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            try
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(()
                    =>
                {
                    extraData = Zd2911Utils.DevicePassGroup;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassGroup, extraProperty, ref device, ref extraData);
                });

                if (result)
                {
                    byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                    for (int index = 0; index < Zd2911Utils.PassItemCount; index++)
                    {
                        var timeGroupService = new TimeGroupService()
                        {
                            TimeGroupId = index,
                            TimeSegmentIds = ZeroToNineCollection.Select(num => (int)data[index + num]),
                        };

                        timeGroupServices.Add(timeGroupService);
                    }
                }

                return timeGroupServices;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool BatchUpdateTimeGroupServices(IEnumerable<TimeGroupService> services)
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
                    extraData = Zd2911Utils.DevicePassGroup;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassGroup, extraProperty, ref device, ref extraData);
                });
                if (result == false)
                {
                    throw new Exception("cannot get timegroups");
                }

                byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                foreach (var timeGroupService in services)
                {
                    Int32 index = timeGroupService.TimeGroupId * 10;
                    foreach (var timeSegmentId in timeGroupService.TimeSegmentIds)
                    {
                        data[index++] = (byte)timeSegmentId;
                    }
                }
                extraData = Encoding.Unicode.GetString(data);

                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                result = retryablePolicy.Execute(
                    () => _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.PassGroup, Zd2911Utils.DevicePassGroup, device, extraData));
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
        #endregion

        #region TimeZone
        public IEnumerable<TimeZoneService> GetAllTimeZoneServices()
        {
            var timeZoneServices = new List<TimeZoneService>();
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();
            try
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                bool result = retryablePolicy.Execute(()
                    =>
                {
                    extraData = Zd2911Utils.DevicePassZone;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassZone, extraProperty, ref device, ref extraData);
                });

                if (result)
                {
                    byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                    for (int index = 0; index < Zd2911Utils.PassItemCount; index++)
                    {
                        var timeZoneService = new TimeZoneService()
                        {
                            TimeZoneId = index,
                            MondayTimeGroupId = data[index + 0],
                            TuesdayTimeGroupId = data[index + 1],
                            WednesdayTimeGroupId = data[index + 2],
                            ThursdayTimeGroupId = data[index + 3],
                            FridayTimeGroupId = data[index + 4],
                            SaturdayTimeGroupId = data[index + 5],
                            SundayTimeGroupId = data[index + 6],
                        };

                        timeZoneServices.Add(timeZoneService);
                    }
                }

                return timeZoneServices;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool BatchUpdateTimeZoneServices(IEnumerable<TimeZoneService> services)
        {
            bool result = false;
            var device = _deviceProxy.Device;
            var extraProperty = new object();
            var extraData = new object();

            try
            {
                var retryablePolicy = Policies.GetRetryablePolicy();
                result = retryablePolicy.Execute(
                () =>
                {
                    extraData = Zd2911Utils.DevicePassZone;
                    return _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.PassZone, extraProperty, ref device, ref extraData);
                });
                if (result == false)
                {
                    throw new Exception("cannot get timezones");
                }

                byte[] data = Encoding.Unicode.GetBytes((string)extraData);
                foreach (var timeZoneServices in services)
                {
                    Int32 index = timeZoneServices.TimeZoneId * 7;
                    if (timeZoneServices.MondayTimeGroupId.HasValue) data[index + 0] = (byte)timeZoneServices.MondayTimeGroupId;
                    if (timeZoneServices.TuesdayTimeGroupId.HasValue) data[index + 1] = (byte)timeZoneServices.TuesdayTimeGroupId;
                    if (timeZoneServices.WednesdayTimeGroupId.HasValue) data[index + 2] = (byte)timeZoneServices.WednesdayTimeGroupId;
                    if (timeZoneServices.ThursdayTimeGroupId.HasValue) data[index + 3] = (byte)timeZoneServices.ThursdayTimeGroupId;
                    if (timeZoneServices.FridayTimeGroupId.HasValue) data[index + 4] = (byte)timeZoneServices.FridayTimeGroupId;
                    if (timeZoneServices.SaturdayTimeGroupId.HasValue) data[index + 5] = (byte)timeZoneServices.SaturdayTimeGroupId;
                    if (timeZoneServices.SundayTimeGroupId.HasValue) data[index + 6] = (byte)timeZoneServices.SundayTimeGroupId;
                }
                extraData = Encoding.Unicode.GetString(data);

                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                result = retryablePolicy.Execute(
                    () => _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.PassZone, Zd2911Utils.DevicePassZone, device, extraData));
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
        #endregion
    }
}