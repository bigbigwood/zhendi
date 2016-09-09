using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Host
{
    public partial class Service1 : ServiceBase
    {
        private DeviceAdapter device;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var config = ConfigurationHelper.CreateConfiguration();
            device = new Rld.DeviceSystem.DeviceAdapter.ZDC2911.DeviceAdapter();
            if (!device.Start(config))
            {
                throw new Exception("device starts fails");
            }
        }

        protected override void OnStop()
        {
            device.Stop();
        }
    }
}
