using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class UserPropertyInfoValidator : AbstractValidator<UserProperty> 
    {
        public UserPropertyInfoValidator()
        {
            RuleFor(m => m.IDNumber).Must(ValidatorToolkit.BeAValidPostcode).Length(18, 25).WithMessage("身份证长度为18-25");
            RuleFor(m => m.Postcode).Must(ValidatorToolkit.BeAValidPostcode).WithMessage("请输入有效邮编号码");
            RuleFor(m => m.Email).EmailAddress().When(m => !string.IsNullOrWhiteSpace(m.Email)).WithMessage("邮箱地址输入有误");
        }

    }
}
