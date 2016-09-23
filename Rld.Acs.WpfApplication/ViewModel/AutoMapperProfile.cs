using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
            : base()
        {
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }

        protected override void Configure()
        {
            AllowNullCollections = true;

            ProvisioningModelMapper.BindModelMap();
        }
    }

    public static class ProvisioningModelMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly String ProfileName = typeof(AutoMapperProfile).Name;

        public static void BindModelMap()
        {
            CreateProvMap<DeviceDoor, DeviceDoorViewModel>()
                .ForMember(dest => dest.IsSelected, op => op.Ignore())
                .ForMember(dest => dest.RingTypeDict, op => op.Ignore())
                .ForMember(dest => dest.CheckOutOptionsDict, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.ModifyDoorCmd, op => op.Ignore());

            CreateProvMap<DeviceDoorViewModel, DeviceDoor>();

            CreateProvMap<DeviceHeadReading, DeviceHeadReadingViewModel>()
                .ForMember(dest => dest.IsSelected, op => op.Ignore())
                .ForMember(dest => dest.HeadReaderTypeDict, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.ModifyHeadReadingCmd, t => t.Ignore());

            CreateProvMap<DeviceHeadReadingViewModel, DeviceHeadReading>();

            CreateProvMap<DeviceExtensionViewModel, DeviceControllerParameter>();
            CreateProvMap<DeviceControllerParameter, DeviceExtensionViewModel>();

            CreateProvMap<DeviceController, DeviceViewModel>()
                .ForMember(dest => dest.Id, op => op.MapFrom(src => src.DeviceID))
                .ForMember(dest => dest.DeviceExtensionViewModel, op => op.MapFrom(src => src.DeviceControllerParameter))
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.Title, op => op.Ignore())
                .ForMember(dest => dest.DoorViewModels, op => op.Ignore())
                .ForMember(dest => dest.HeadReadingViewModels, op => op.Ignore())
                .ForMember(dest => dest.CommunicationTypeDict, op => op.Ignore())
                .ForMember(dest => dest.ProtocolDict, op => op.Ignore())
                .ForMember(dest => dest.AuthticationTypeDict, op => op.Ignore())
                .ForMember(dest => dest.Timezones, op => op.Ignore())
                .ForMember(dest => dest.ViewModelAttachment, op => op.Ignore())
                .ForMember(dest => dest.DoorListString, op => op.Ignore())
                .ForMember(dest => dest.HeadReadingListString, op => op.Ignore());

            CreateProvMap<DeviceViewModel, DeviceController>()
                .ForMember(dest => dest.DeviceID, op => op.MapFrom(x => x.Id))
                .ForMember(dest => dest.DeviceControllerParameter, op => op.MapFrom(src => src.DeviceExtensionViewModel))
                .ForMember(dest => dest.DeviceDoors, op => op.Ignore())
                .ForMember(dest => dest.DeviceHeadReadings, op => op.Ignore());

            CreateProvMap<DeviceTrafficLogViewModel, DeviceTrafficLog>();
            CreateProvMap<DeviceTrafficLog, DeviceTrafficLogViewModel>()
                 .ForMember(dest => dest.AuthenticationString, op => op.Ignore());

            CreateProvMap<DeviceOperationLogViewModel, DeviceOperationLog>();
            CreateProvMap<DeviceOperationLog, DeviceOperationLogViewModel>();

            CreateProvMap<SysOperationLogViewModel, SysOperationLog>();
            CreateProvMap<SysOperationLog, SysOperationLogViewModel>();

            CreateProvMap<SysDictionaryViewModel, SysDictionary>();
            CreateProvMap<SysDictionary, SysDictionaryViewModel>()
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.TypeHeadersDict, op => op.Ignore());

            CreateProvMap<SysRoleViewModel, SysRole>()
                .ForMember(dest => dest.SysRolePermissions, op => op.MapFrom(src => src.GetPermissionsFromUI()));
            CreateProvMap<SysRole, SysRoleViewModel>()
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.Title, op => op.Ignore())
                .ForMember(dest => dest.AuthorizationModuleString, op => op.MapFrom(src => src.GetModuleString()))
                .ForMember(dest => dest.ViewModelAttachment, op => op.Ignore())
                .ForMember(dest => dest.TreeViewSource, op => op.Ignore())
                .AfterMap((src, dest) => dest.BindPermissionsToTreeView(src.SysRolePermissions));

            CreateProvMap<SysOperatorViewModel, SysOperator>()
                .ForMember(dest => dest.SysOperatorRoles, op => op.Ignore())
                .ForMember(dest => dest.Status, op => op.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.SysOperatorRoles = src.GetRolesFromUI(dest);
                    dest.Status = src.Status ? GeneralStatus.Enabled : GeneralStatus.Disabled;
                });
            CreateProvMap<SysOperator, SysOperatorViewModel>()
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.NewPasswordEnabled, op => op.Ignore())
                .ForMember(dest => dest.NewPassword1, op => op.Ignore())
                .ForMember(dest => dest.NewPassword2, op => op.Ignore())
                .ForMember(dest => dest.Title, op => op.Ignore())
                .ForMember(dest => dest.ViewModelAttachment, op => op.Ignore())
                .ForMember(dest => dest.StatusInfo, op => op.Ignore())
                .ForMember(dest => dest.SysOperatorRoleItems, op => op.Ignore())
                .ForMember(dest => dest.Status, op => op.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.BindUI(src);
                });

            CreateProvMap<FloorDoorViewModel, FloorDoor>();
            CreateProvMap<FloorDoor, FloorDoorViewModel>()
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.DoorName, op => op.Ignore())
                .ForMember(dest => dest.Enabled, op => op.Ignore())
                ;

            CreateProvMap<FloorViewModel, Floor>()
                 .ForMember(dest => dest.Doors, op => op.Ignore());
            CreateProvMap<Floor, FloorViewModel>()
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.Doors, op => op.Ignore())
                .ForMember(dest => dest.DoorNames, op => op.Ignore())
                .AfterMap((src, dest) => dest.BindDoors(src.Doors));

            CreateProvMap<DataSyncConfigViewModel, SysConfig>();
            CreateProvMap<SysConfig, DataSyncConfigViewModel>()
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.IsSelected, op => op.Ignore());

            CreateProvMap<UserAuthenticationViewModel, UserAuthentication>()
                .ForMember(dest => dest.AuthenticationData, op => op.MapFrom(src => src.AuthenticationData.Replace(' ', '-')));
            CreateProvMap<UserAuthentication, UserAuthenticationViewModel>()
                .ForMember(dest => dest.AuthenticationData, op => op.MapFrom(src => src.AuthenticationData.Replace('-', ' ')))
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.IsSelected, op => op.Ignore())
                .ForMember(dest => dest.Name, op => op.Ignore())
                .ForMember(dest => dest.ModifyCmd, op => op.Ignore());

            Log.Info("Verify mapper configuration..");
            Mapper.AssertConfigurationIsValid();
        }

        private static void CreateTwoWayMap<T1, T2>()
        {
            Mapper.CreateMap<T1, T2>().WithProfile(ProfileName);
            Mapper.CreateMap<T2, T1>().WithProfile(ProfileName);
        }

        private static IMappingExpression<T1, T2> CreateProvMap<T1, T2>()
        {
            return Mapper.CreateMap<T1, T2>()
                .WithProfile(ProfileName);
        }
    }
}
