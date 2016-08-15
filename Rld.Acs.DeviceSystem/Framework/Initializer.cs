using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.Acs.DeviceSystem.Framework
{
    public class Initializer
    {
        public static void Initialize()
        {
            Repository.RepositoryManager.AddAssemby(typeof(Repository.Mybatis.MsSql.NinjectBinder).Assembly);
            OperationManager.GetInstance();
            WebSocketClientManager.GetInstance();
        }
    }
}