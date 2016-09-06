using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Rld.Acs.Backend.Service;

namespace Rld.Acs.Backend
{
    public partial class Service1 : ServiceBase
    {
        private BackendService service;
        public Service1()
        {
            InitializeComponent();
            service = new BackendService();
        }

        protected override void OnStart(string[] args)
        {
            service.OnStart();
        }

        protected override void OnStop()
        {
            service.OnStop();
        }
    }
}
