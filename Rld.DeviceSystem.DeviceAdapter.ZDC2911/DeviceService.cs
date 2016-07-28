using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Riss.Devices;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911
{
    public class DeviceService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public bool SetUserName(int userId, string userName)
        //{
        //    try
        //    {
        //        var deviceProxy = DeviceManager.GetInstance().GetDeviceProxy();

        //        object extraProperty = new object();
        //        object extraData = new object();
        //        var user = new User() { DIN = (UInt64)userId, UserName = userName };
        //        bool result = deviceProxy.DeviceConnection.SetProperty(UserProperty.UserName, extraProperty, user, extraData);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return false;
        //    }
        //}

        //public string GetUserName()
        //{
        //    try
        //    {
        //        var deviceProxy = DeviceManager.GetInstance().GetDeviceProxy();

        //        var extraData = new object();
        //        var user = new User() { DIN = (UInt64)deviceProxy.Device.DN };
        //        bool result = deviceProxy.DeviceConnection.GetProperty(UserProperty.UserName, new object(), ref user, ref extraData);
        //        if (!result)
        //        {
        //            Log.Error("Get user name fails.");
        //            return "";
        //        }

        //        return user.UserName;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return "";
        //    }
        //}
    }
}