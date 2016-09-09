using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Rld.DeviceSystem.Contract.Model.Configuration;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Configuration;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Host
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            if (args.Length == 1 && args[0] == "-console")
            {
                try
                {
                    System.Console.WriteLine("presss enter to run!");
                    System.Console.ReadLine();
                    System.Console.WriteLine("Starting.");

                    var config = ConfigurationHelper.CreateConfiguration();
                    var device = new Rld.DeviceSystem.DeviceAdapter.ZDC2911.DeviceAdapter();
                    bool result = device.Start(config);
                    if (!result)
                    {
                        System.Console.WriteLine("device start fails!");
                    }

                    System.Console.WriteLine("presss enter to close!");
                    System.Console.ReadLine();
                    device.Stop();
                    System.Console.WriteLine("closed.");
                    System.Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("device start fails:" + ex.Message);
                }
            }
            else
            {
                ServiceBase[] ServicesToRun = new ServiceBase[] { new Service1() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
