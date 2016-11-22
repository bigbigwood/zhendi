using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rld.Acs.Unility;

namespace Rld.Acs.WpfApplication.Service.Lisence
{
    public static class LisenceService
    {
        public static bool RequestLisence()
        {
            var id = string.Format("{0}-{1}", LocalMachineInfomationProvider.GetHostname(), LocalMachineInfomationProvider.GetMac());
            var ip = LocalMachineInfomationProvider.GetLocalIp();
            var lisenceService = new LisenceServiceProxy();
            var tryOnlineResult = lisenceService.Online(id, ip);
            return tryOnlineResult.ResultType == ResultType.OK;
        }

        public static bool ReleaseLisence()
        {
            var id = string.Format("{0}-{1}", LocalMachineInfomationProvider.GetHostname(), LocalMachineInfomationProvider.GetMac());
            var ip = LocalMachineInfomationProvider.GetLocalIp();
            var lisenceService = new LisenceServiceProxy();
            var tryOnlineResult = lisenceService.Offline(id, ip);
            return tryOnlineResult.ResultType == ResultType.OK;
        }
    }
}
