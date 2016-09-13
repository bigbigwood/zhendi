using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class DoorValidator: AbstractValidator<DeviceDoor> 
    {
        public DoorValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("门名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("门名称不能有特殊字符");
            
            RuleFor(m => m.Code)
                .NotEmpty().WithMessage("门编号不能为空")
                .Must(ValidatorToolkit.IsNumberOrChar).WithMessage("门编号只能为数字或者字母");
        }
    }
}
