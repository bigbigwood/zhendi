using Ninject.Modules;
using Rld.Acs.Repository.Framework;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class NinjectBinder : NinjectModule
    {
        public override void Load()
        {
            Bind<IPersistanceConnection>().To<SessionToPersistanceAdapter>();
            Bind<IConnectionProvider>().To<MyBatisConnectionProvider>();
            Bind<IPersistanceTransaction>().To<TransactionToPersistanceTransaction>();

            Bind<ICustomerRepository>().To<CustomerRepositoryMB>();
        }
    }
}
