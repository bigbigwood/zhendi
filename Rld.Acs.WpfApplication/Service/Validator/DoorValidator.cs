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
                .Length(1, 100).WithMessage("门名称长度为1-100")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("门名称:[{0}]不能有特殊字符", x => x.Name);

            RuleFor(m => m.Code)
                .NotEmpty().WithMessage("门编号不能为空")
                .Must(ValidatorToolkit.IsNumeric).WithMessage("门编号只能为数字");

            RuleFor(m => m.Remark)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.Remark)).WithMessage("备注长度为1-1024");

            RuleFor(m => m.DuressPassword)
                .Length(1, 100).When(m => !string.IsNullOrWhiteSpace(m.DuressPassword)).WithMessage("胁迫密码长度为1-100");
        }
    }
}
