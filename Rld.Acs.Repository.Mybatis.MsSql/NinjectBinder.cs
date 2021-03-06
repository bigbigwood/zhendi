﻿using Ninject.Modules;
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

            //Department
            Bind<IDepartmentRepository>().To<DepartmentRepositoryMB>();
            Bind<IDepartmentDeviceRepository>().To<DepartmentDeviceRepositoryMB>();
            //Device
            Bind<IDeviceRoleRepository>().To<DeviceRoleRepositoryMB>();
            Bind<IDeviceRolePermissionRepository>().To<DeviceRolePermissionRepositoryMB>();
            Bind<IDeviceControllerRepository>().To<DeviceControllerRepositoryMB>();
            Bind<IDeviceControllerParameterRepository>().To<DeviceControllerParameterRepositoryMB>();
            Bind<IDeviceDoorRepository>().To<DeviceDoorRepositoryMB>();
            Bind<IDeviceHeadReadingRepository>().To<DeviceHeadReadingRepositoryMB>();
            Bind<IDeviceOperationLogRepository>().To<DeviceOperationLogRepositoryMB>();
            Bind<IDeviceStateHistoryRepository>().To<DeviceStateHistoryRepositoryMB>();
            Bind<IDeviceTrafficLogRepository>().To<DeviceTrafficLogRepositoryMB>();
            Bind<IDeviceGroupRepository>().To<DeviceGroupRepositoryMB>();
            //System
            Bind<ISysDictionaryRepository>().To<SysDictionaryRepositoryMB>();
            Bind<ISysModuleElementRepository>().To<SysModuleElementRepositoryMB>();
            Bind<ISysModuleRepository>().To<SysModuleRepositoryMB>();
            Bind<ISysOperationLogRepository>().To<SysOperationLogRepositoryMB>();
            Bind<ISysOperatorRepository>().To<SysOperatorRepositoryMB>();
            Bind<ISysRoleRepository>().To<SysRoleRepositoryMB>();
            Bind<ISysRolePermissionRepository>().To<SysRolePermissionRepositoryMB>();
            Bind<ISysOperatorRoleRepository>().To<SysOperatorRoleRepositoryMB>();
            Bind<ISysConfigRepository>().To<SysConfigRepositoryMB>();
            //Time
            Bind<ITimeGroupRepository>().To<TimeGroupRepositoryMB>();
            Bind<ITimeGroupSegmentRepository>().To<TimeGroupSegmentRepositoryMB>();
            Bind<ITimeSegmentRepository>().To<TimeSegmentRepositoryMB>();
            Bind<ITimeZoneRepository>().To<TimeZoneRepositoryMB>();
            Bind<ITimeZoneGroupRepository>().To<TimeZoneGroupRepositoryMB>();
            //User
            Bind<IUserRepository>().To<UserRepositoryMB>();
            Bind<IUserPropertyRepository>().To<UserPropertyRepositoryMB>();
            Bind<IUserAuthenticationRepository>().To<UserAuthenticationRepositoryMB>();
            Bind<IUserDeviceRoleRepository>().To<UserDeviceRoleRepositoryMB>();
            Bind<IUserEventRepository>().To<UserEventRepositoryMB>();
            //Floor
            Bind<IFloorRepository>().To<FloorRepositoryMB>();
            Bind<IFloorDoorRepository>().To<FloorDoorRepositoryMB>();
        }
    }
}
