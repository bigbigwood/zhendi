using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model
{
    public class DeviceStatus
    {
                /// <summary>
        /// 设备处于繁忙状态
        /// </summary>
        public const long DeviceBusy = 1;

        /// <summary>
        /// 设备处于空闲状态
        /// </summary>
        public const long DeviceIdle = 0;
    }
}