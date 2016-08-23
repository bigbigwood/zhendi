using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class DeviceRoleValidator: AbstractValidator<DeviceRole> 
    {
        public DeviceRoleValidator()
        {
            RuleFor(m => m.RoleName)
                .NotEmpty().WithMessage("设备名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("设备名称不能有特殊字符");
        }
    }
}
