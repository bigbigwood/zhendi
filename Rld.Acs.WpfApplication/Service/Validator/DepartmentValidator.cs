using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class DepartmentValidator: AbstractValidator<Department> 
    {
        public DepartmentValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("部门名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("部门名称不能有特殊字符");

            RuleFor(m => m.DepartmentCode)
                .NotEmpty().WithMessage("部门编号不能为空")
                .Must(ValidatorToolkit.IsNumberOrChar).WithMessage("部门编号只能为数字或者字母");
        }
    }
}
