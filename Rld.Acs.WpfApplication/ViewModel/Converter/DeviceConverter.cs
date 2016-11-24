using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Rld.Acs.Model;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Service.Language;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Converter
{
    public static class DeviceConverter
    {
        public static DeviceViewModel ToViewModel(this DeviceController t)
        {
            var deviceViewModel = AutoMapper.Mapper.Map<DeviceViewModel>(t);
            deviceViewModel.DoorViewModels = new ObservableCollection<DeviceDoorViewModel>();
            for (int i = 1; i <= 6; i++)
            {
                deviceViewModel.DoorViewModels.Add(new DeviceDoorViewModel() { DeviceID = t.DeviceID, 
                    Name = LanguageManager.GetLocalizationResource(Resource.Door) + " " + i });
            }

            deviceViewModel.HeadReadingViewModels = new ObservableCollection<DeviceHeadReadingViewModel>();
            for (int i = 1; i <= 6; i++)
            {
                deviceViewModel.HeadReadingViewModels.Add(new DeviceHeadReadingViewModel() { DeviceID = t.DeviceID, 
                    Name = LanguageManager.GetLocalizationResource(Resource.HeadReader) + "　" +　i });
            }

            for (int i = 0; i < t.DeviceDoors.Count; i++)
            {
                deviceViewModel.DoorViewModels[i] = Mapper.Map<DeviceDoorViewModel>(t.DeviceDoors[i]);
                deviceViewModel.DoorViewModels[i].IsSelected = true;
            }
            for (int i = 0; i < t.DeviceHeadReadings.Count; i++)
            {
                deviceViewModel.HeadReadingViewModels[i] = Mapper.Map<DeviceHeadReadingViewModel>(t.DeviceHeadReadings[i]);
                deviceViewModel.HeadReadingViewModels[i].IsSelected = true;
            }
            return deviceViewModel;
        }

        public static DeviceController ToCoreModel(this DeviceViewModel t)
        {
            var deviceController = AutoMapper.Mapper.Map<DeviceController>(t);

            var enabledDoorVMs = t.DoorViewModels.FindAll(x => x.IsSelected);
            deviceController.DeviceDoors = enabledDoorVMs.Select(Mapper.Map<DeviceDoor>).ToList();

            var enabledHeadReadingVMs = t.HeadReadingViewModels.FindAll(x => x.IsSelected);
            deviceController.DeviceHeadReadings = enabledHeadReadingVMs.Select(Mapper.Map<DeviceHeadReading>).ToList();

            return deviceController;
        }
    }
}
