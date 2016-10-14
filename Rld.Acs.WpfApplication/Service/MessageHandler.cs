using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rld.Acs.WpfApplication.DeviceProxy;

namespace Rld.Acs.WpfApplication.Service
{
    public class MessageHandler
    {
        public static string GenerateDeviceMessage(ResultTypes resultTypes, string[] messages, string okMessage = "", string failureMessage = "")
        {
            var message = "";
            if (resultTypes == ResultTypes.Ok)
                message = okMessage;
            else if (resultTypes == ResultTypes.NotSupportError)
                message = "当前设备无法支持此功能";
            else if (resultTypes == ResultTypes.DeviceNotConnected)
                message = messages.FirstOrDefault();
            else
                message = failureMessage;

            return message;
        }
    }
}
