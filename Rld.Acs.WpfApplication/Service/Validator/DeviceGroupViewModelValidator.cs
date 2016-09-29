using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class DeviceGroupViewModelValidator : AbstractValidator<DeviceGroupViewModel> 
    {
        public DeviceGroupViewModelValidator()
        {
            RuleFor(m => m.DeviceGroupName)
                .NotEmpty().WithMessage("设备组名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("设备组名称:[{0}]不能有特殊字符", x => x.DeviceGroupName);

            RuleFor(m => m.CheckInDeviceID)
                .NotEqual(m => m.CheckOutDeviceID)
                .WithMessage("出门设备和入门设备不能相同");
        }
    }
}
