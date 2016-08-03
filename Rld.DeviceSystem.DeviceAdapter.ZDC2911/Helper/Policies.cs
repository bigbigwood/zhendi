using System;
using System.Reflection;
using log4net;
using Polly;
using Polly.Retry;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper
{
    public class Policies
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static RetryPolicy<Boolean> GetRetryablePolicy()
        {
            return Policy.HandleResult<bool>(b=> b == false)
                .Retry(3 ,
                (ex, timeSpan, context) =>
                    Log.Info("Device reutrn failure, will retry"));
        }
    }
}
