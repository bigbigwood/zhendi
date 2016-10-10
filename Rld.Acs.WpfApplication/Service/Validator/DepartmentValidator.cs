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
                .Length(1, 100).WithMessage("部门名称长度为1-100")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("部门名称不能有特殊字符");

            RuleFor(m => m.DepartmentCode)
                .NotEmpty().WithMessage("部门编号不能为空")
                 .Length(1, 25).WithMessage("部门编号长度为1-25")
                .Must(ValidatorToolkit.IsNumberOrChar).WithMessage("部门编号只能为数字或者字母");

            RuleFor(m => m.Remark)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.Remark)).WithMessage("备注长度为1-1024");
        }
    }
}
