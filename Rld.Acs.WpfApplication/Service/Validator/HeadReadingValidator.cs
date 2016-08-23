using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class HeadReadingValidator: AbstractValidator<DeviceHeadReading> 
    {
        public HeadReadingValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("设备名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("设备名称不能有特殊字符");
            
            RuleFor(m => m.Code)
                .NotEmpty().WithMessage("设备编号不能为空")
                .Must(ValidatorToolkit.IsNumberOrChar).WithMessage("设备编号只能为数字或者字母");

            RuleFor(m => m.Mac)
                .NotEmpty().WithMessage("Mac地址不能为空");

            RuleFor(m => m.HeadReadingSN)
                .NotEmpty().WithMessage("产品序列号不能为空");
        }
    }
}
