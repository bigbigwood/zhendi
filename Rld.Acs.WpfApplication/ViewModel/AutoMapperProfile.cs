using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
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
                .ForMember(dest => dest.CancelCmd, op => op.Ignore())
                .ForMember(dest => dest.SaveCmd, op => op.Ignore())
                .ForMember(dest => dest.ModifyDoorCmd, op => op.Ignore());

            CreateProvMap<DeviceDoorViewModel, DeviceDoor>();

            CreateProvMap<DeviceHeadReading, DeviceHeadReadingViewModel>()
                .ForMember(dest => dest.IsSelected, op => op.Ignore())
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
                .ForMember(dest => dest.DoorListString, op => op.MapFrom(src => src.GetDeviceAssociatedDoorList()))
                .ForMember(dest => dest.HeadReadingListString, op => op.MapFrom(src => src.GetDeviceAssociatedHeadReadingList()));

            CreateProvMap<DeviceViewModel, DeviceController>()
                .ForMember(dest => dest.DeviceID, op => op.MapFrom(x => x.Id))
                .ForMember(dest => dest.DeviceControllerParameter, op => op.MapFrom(src => src.DeviceExtensionViewModel))
                .ForMember(dest => dest.DeviceDoors, op => op.Ignore())
                .ForMember(dest => dest.DeviceHeadReadings, op => op.Ignore());

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
