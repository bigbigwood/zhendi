using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Backend.Service;

namespace Rld.Acs.Backend
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        private static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Repository.RepositoryManager.AddAssemby(typeof(Repository.Mybatis.MsSql.NinjectBinder).Assembly);

            if (args.Length == 1 && args[0] == "-console")
            {
                try
                {
                    System.Console.WriteLine("presss enter to run!");
                    System.Console.ReadLine();
                    System.Console.WriteLine("Starting.");

                    var service = new BackendService();
                    service.OnStart();

                    System.Console.WriteLine("presss enter to close!");
                    System.Console.ReadLine();
                    service.OnStop();
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
                ServiceBase[] ServicesToRun = new ServiceBase[] {new Service1()};
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
