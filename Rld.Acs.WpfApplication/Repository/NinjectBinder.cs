using Ninject;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rld.Acs.WpfApplication.Service.Validator;

namespace Rld.Acs.WpfApplication.Repository
{
    public static class NinjectBinder
    {
        private static readonly IKernel InjectionKernel;

        static NinjectBinder()
        {
            InjectionKernel = new StandardKernel();

            BindRepository();
            BindValidator();
        }

        private static void BindRepository()
        {
            InjectionKernel.Bind<IUserRepository>().To<UserRepository>();
            InjectionKernel.Bind<IDepartmentRepository>().To<DepartmentRepository>();
            InjectionKernel.Bind<IDepartmentDeviceRepository>().To<DepartmentDeviceRepository>();
            InjectionKernel.Bind<IDeviceRoleRepository>().To<DeviceRoleRepository>();
            InjectionKernel.Bind<IDeviceControllerRepository>().To<DeviceControllerRepository>();

            InjectionKernel.Bind<ITimeSegmentRepository>().To<TimeSegmentRepository>();
            InjectionKernel.Bind<ITimeGroupRepository>().To<TimeGroupRepository>();
            InjectionKernel.Bind<ITimeZoneRepository>().To<TimeZoneRepository>();

            InjectionKernel.Bind<ISysDictionaryRepository>().To<SysDictionaryRepository>();
        }

        private static void BindValidator()
        {
            InjectionKernel.Bind<UserValidator>().To<UserValidator>();
            InjectionKernel.Bind<UserPropertyInfoValidator>().To<UserPropertyInfoValidator>();
        }

        public static TRepositoryOfEntity GetRepository<TRepositoryOfEntity>()
        {
            return (InjectionKernel.Get<TRepositoryOfEntity>());
        }

        public static TValidator GetValidator<TValidator>()
        {
            return (InjectionKernel.Get<TValidator>());
        }
    }
}
