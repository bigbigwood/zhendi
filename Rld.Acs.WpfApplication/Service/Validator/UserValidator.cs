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
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("人员名称不能有特殊字符");
            
            RuleFor(m => m.UserCode)
                .NotEmpty().WithMessage("人员编号不能为空")
                .Must(ValidatorToolkit.IsNumeric).WithMessage("人员编号只能为数字");
            
            RuleFor(m => m.Phone)
                .Must(ValidatorToolkit.IsPhoneNumberFormat).When(m => !string.IsNullOrWhiteSpace(m.Phone)).WithMessage("号码格式错误")
                .Length(7,13).When(m => !string.IsNullOrWhiteSpace(m.Phone)).WithMessage("号码长度为7-13");

            RuleFor(m => m.StartDate.Date)
                .LessThan(m => m.EndDate.Value.Date).When(m => m.EndDate.HasValue).WithMessage("开始时间必须小于结束时间");

            RuleFor(m => m.UserPropertyInfo).SetValidator(NinjectBinder.GetValidator<UserPropertyInfoValidator>());
        }
    }
}
