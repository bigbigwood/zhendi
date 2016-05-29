using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;

namespace Rld.Acs.Repository
{
    public static class NinjectBinder
    {
        private static IKernel _ninjectKernel;

        public static void Initialize()
        {
            _ninjectKernel = new StandardKernel();

            //_ninjectKernel.Bind<IPersistanceConnection>().To<SessionToPersistanceAdapter>();
            //_ninjectKernel.Bind<IConnectionProvider>().To<MyBatisConnectionProvider>();
            //_ninjectKernel.Bind<IPersistanceTransaction>().To<TransactionToPersistanceTransaction>();
            //_ninjectKernel.Bind<ISequenceProvider>().To<MyBatisSequenceProvider>();

            //_ninjectKernel.Bind<IOrderRepository>().To<OrderRepositoryMB>();
        }

        public static TInterface Get<TInterface>()
        {
            return _ninjectKernel.Get<TInterface>();
        }
    }
}