using Ninject;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rld.Acs.WpfApplication.Repository.System;
using Rld.Acs.WpfApplication.Service.Validator;
using RldModel = Rld.Acs.Model;

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
            InjectionKernel.Bind<IDeviceTrafficLogRepository>().To<DeviceTrafficLogRepository>();
            InjectionKernel.Bind<IDeviceOperationLogRepository>().To<DeviceOperationLogRepository>();

            InjectionKernel.Bind<ITimeSegmentRepository>().To<TimeSegmentRepository>();
            InjectionKernel.Bind<ITimeGroupRepository>().To<TimeGroupRepository>();
            InjectionKernel.Bind<ITimeZoneRepository>().To<TimeZoneRepository>();

            InjectionKernel.Bind<ISysDictionaryRepository>().To<SysDictionaryRepository>();
            InjectionKernel.Bind<ISysRoleRepository>().To<SysRoleRepository>();
            InjectionKernel.Bind<ISysModuleRepository>().To<SysModuleRepository>();
            InjectionKernel.Bind<ISysModuleElementRepository>().To<SysModuleElementRepository>();
            InjectionKernel.Bind<ISysOperatorRepository>().To<SysOperatorRepository>();
            InjectionKernel.Bind<ISysOperationLogRepository>().To<SysOperationLogRepository>();
            InjectionKernel.Bind<ISysConfigRepository>().To<SysConfigRepository>();

            InjectionKernel.Bind<IFloorRepository>().To<FloorRepository>();
        }

        private static void BindValidator()
        {
            InjectionKernel.Bind<UserValidator>().To<UserValidator>();
            InjectionKernel.Bind<UserPropertyInfoValidator>().To<UserPropertyInfoValidator>();
            InjectionKernel.Bind<Department>().To<Department>();
            InjectionKernel.Bind<TimeSegmentViewModelValidator>().To<TimeSegmentViewModelValidator>();
            InjectionKernel.Bind<TimeGroupViewModelValidator>().To<TimeGroupViewModelValidator>();
            InjectionKernel.Bind<TimeZoneViewModelValidator>().To<TimeZoneViewModelValidator>();
            InjectionKernel.Bind<DeviceValidator>().To<DeviceValidator>();
            InjectionKernel.Bind<DeviceParameterValidator>().To<DeviceParameterValidator>();
            InjectionKernel.Bind<DoorValidator>().To<DoorValidator>();
            InjectionKernel.Bind<HeadReadingValidator>().To<HeadReadingValidator>();
            InjectionKernel.Bind<DeviceRoleViewModelValidator>().To<DeviceRoleViewModelValidator>();
            InjectionKernel.Bind<SysDictionaryValidator>().To<SysDictionaryValidator>();
            InjectionKernel.Bind<FloorValidator>().To<FloorValidator>();

            InjectionKernel.Bind<SyncUserViewModelValidator>().To<SyncUserViewModelValidator>();
            InjectionKernel.Bind<UserAuthenticationViewModelValidator>().To<UserAuthenticationViewModelValidator>();
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
