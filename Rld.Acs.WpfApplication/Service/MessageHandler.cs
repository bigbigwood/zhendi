using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.DeviceProxy;
using Rld.Acs.WpfApplication.Service.Language;

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
                message = LanguageManager.GetLocalizationResource(Resource.MSG_DeviceNotSupportError);
            else if (resultTypes == ResultTypes.DeviceNotConnected)
            {
                var offlineDeviceIds = messages.FirstOrDefault();
                message = LanguageManager.GetLocalizationResourceFormat(Resource.MSG_DeviceIsNotConnected, offlineDeviceIds);
            }
            else
                message = failureMessage;

            return message;
        }
    }
}
