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
                .NotEmpty().WithMessage("读头名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("读头名称:[{0}]不能有特殊字符", x => x.Name);
            
            RuleFor(m => m.Code)
                .NotEmpty().WithMessage("读头编号不能为空")
                .Must(ValidatorToolkit.IsNumberOrChar).WithMessage("读头编号只能为数字或者字母");

            RuleFor(m => m.Mac)
                .NotEmpty().WithMessage("读头Mac地址不能为空");

            RuleFor(m => m.HeadReadingSN)
                .NotEmpty().WithMessage("读头产品序列号不能为空");
        }
    }
}
