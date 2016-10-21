using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using Rld.Acs.Unility.Extension;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Framework
{
    public class HealthThread
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public readonly int HealthCheckSecond = 5 * 60;
        private readonly System.Timers.Timer _timer;
        private readonly Func<Boolean> _healthCheckFunc;
        private readonly Func<Boolean> _healthFixFunc;

        public HealthThread(Func<Boolean> healthCheckFunc, Func<Boolean> healthFixFunc)
        {
            var healthCheckSecondConfig = ConfigurationManager.AppSettings["HealthCheckSecond"];
            if (!string.IsNullOrWhiteSpace(healthCheckSecondConfig))
            {
                HealthCheckSecond = healthCheckSecondConfig.ToInt32();
            }

            _healthCheckFunc = healthCheckFunc;
            _healthFixFunc = healthFixFunc;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += HealthCheck;
            _timer.Interval = HealthCheckSecond * 1000;
        }

        private void HealthCheck(object source, ElapsedEventArgs e)
        {
            try
            {
                Log.Info("HealthThread check triggers.");
                if (_healthCheckFunc != null)
                {
                    var checkResult = _healthCheckFunc.Invoke();
                    Log.InfoFormat("HealthThread check result: {0}.", checkResult);

                    if (!checkResult)
                    {
                        if (_healthFixFunc != null)
                        {
                            var fixResult = _healthFixFunc.Invoke();
                            Log.InfoFormat("HealthThread fix result: {0}.", fixResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void Start()
        {
            Log.InfoFormat("HealthThread started, will check connection with server in every {0} seconds.", HealthCheckSecond);
            //_timer.Enabled = true;
            _timer.Start();
        }

        public void Stop()
        {
            Log.Info("HealthThread stopped.");
            _timer.Stop();
        }
    }
}
