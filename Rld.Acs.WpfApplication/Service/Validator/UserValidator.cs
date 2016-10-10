using System.Data;
using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class UserValidator: AbstractValidator<User> 
    {
        public UserValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("人员名称不能为空")
                .Length(1, 100).WithMessage("人员名称长度为1-100")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("人员名称不能有特殊字符");
            
            RuleFor(m => m.UserCode)
                .NotEmpty().WithMessage("人员编号不能为空")
                .Must(ValidatorToolkit.IsNumeric).WithMessage("人员编号只能为数字");
            
            RuleFor(m => m.Phone)
                .NotEmpty().WithMessage("手机号码不能为空")
                .Must(ValidatorToolkit.IsPhoneNumberFormat).WithMessage("号码格式错误")
                .Length(7,13).WithMessage("号码长度为7-13");

            RuleFor(m => m.StartDate.Date)
                .LessThan(m => m.EndDate.Value.Date).When(m => m.EndDate.HasValue).WithMessage("开始时间必须小于结束时间");

            RuleFor(m => m.Remark)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.Remark)).WithMessage("备注长度为1-1024");

            RuleFor(m => m.UserPropertyInfo).SetValidator(NinjectBinder.GetValidator<UserPropertyInfoValidator>());
        }
    }
}
