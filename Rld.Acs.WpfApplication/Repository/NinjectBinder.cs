using Ninject;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Repository
{
    public static class NinjectBinder
    {
        private static readonly IKernel InjectionKernel;

        static NinjectBinder()
        {
            InjectionKernel = new StandardKernel();
            InitBinding();
        }

        private static void InitBinding()
        {
            InjectionKernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            InjectionKernel.Bind<IDepartmentRepository>().To<DepartmentRepository>();
            InjectionKernel.Bind<IDepartmentDeviceRepository>().To<DepartmentDeviceRepository>();
            InjectionKernel.Bind<IDeviceRoleRepository>().To<DeviceRoleRepository>();
            InjectionKernel.Bind<IDeviceControllerRepository>().To<DeviceControllerRepository>();

            InjectionKernel.Bind<ITimeSegmentRepository>().To<TimeSegmentRepository>();
            InjectionKernel.Bind<ITimeGroupRepository>().To<TimeGroupRepository>();
            InjectionKernel.Bind<ITimeZoneRepository>().To<TimeZoneRepository>();
        }

        public static TRepositoryOfEntity GetRepository<TRepositoryOfEntity>()
        {
            return (InjectionKernel.Get<TRepositoryOfEntity>());
        }
    }
}
