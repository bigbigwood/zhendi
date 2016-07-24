using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class UserPropertyInfoValidator : AbstractValidator<UserProperty> 
    {
        public UserPropertyInfoValidator()
        {
            RuleFor(m => m.IDNumber)
                .NotEmpty().WithMessage("身份证不能为空")
                .Must(ValidatorToolkit.IsIDCardFormat).WithMessage("身份证长度为15或18位数字和x字符");

            RuleFor(m => m.Postcode)
                .Must(ValidatorToolkit.IsPostCodeFormat).When(m => !string.IsNullOrWhiteSpace(m.Postcode)).WithMessage("请输入有效邮编号码");
            
            RuleFor(m => m.Email)
                .EmailAddress().When(m => !string.IsNullOrWhiteSpace(m.Email)).WithMessage("邮箱地址格式错误");

            RuleFor(m => m.HomeNumber)
                .Must(ValidatorToolkit.IsPhoneNumberFormat).When(m => !string.IsNullOrWhiteSpace(m.HomeNumber)).WithMessage("号码格式错误")
                .Length(7, 11).When(m => !string.IsNullOrWhiteSpace(m.HomeNumber)).WithMessage("号码长度为7-11");

            RuleFor(m => m.SocialNumber)
                .Must(ValidatorToolkit.IsNumeric).When(m => !string.IsNullOrWhiteSpace(m.SocialNumber)).WithMessage("请输入有效社保号码");
            
        }

    }
}
