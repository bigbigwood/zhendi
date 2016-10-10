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
                .Must(ValidatorToolkit.IsIDCardFormat).WithMessage("身份证长度为15或18位数字和X字符");

            RuleFor(m => m.TechnicalTitle)
                .NotEmpty().WithMessage("部门职称不能为空")
                .Length(1, 50).When(m => !string.IsNullOrWhiteSpace(m.TechnicalTitle)).WithMessage("部门职称长度为1-50");

            RuleFor(m => m.Postcode)
                .Must(ValidatorToolkit.IsPostCodeFormat).When(m => !string.IsNullOrWhiteSpace(m.Postcode)).WithMessage("请输入有效邮编号码");
            
            RuleFor(m => m.Email)
                .EmailAddress().When(m => !string.IsNullOrWhiteSpace(m.Email)).WithMessage("邮箱地址格式错误")
                .Length(1, 100).When(m => !string.IsNullOrWhiteSpace(m.Email)).WithMessage("邮箱地址长度为1-100");

            RuleFor(m => m.HomeNumber)
                .Must(ValidatorToolkit.IsPhoneNumberFormat).When(m => !string.IsNullOrWhiteSpace(m.HomeNumber)).WithMessage("号码格式错误")
                .Length(7, 13).When(m => !string.IsNullOrWhiteSpace(m.HomeNumber)).WithMessage("号码长度为7-13");

            RuleFor(m => m.SocialNumber)
                .Must(ValidatorToolkit.IsNumeric).When(m => !string.IsNullOrWhiteSpace(m.SocialNumber)).WithMessage("请输入有效社保号码")
                .Length(1, 50).When(m => !string.IsNullOrWhiteSpace(m.SocialNumber)).WithMessage("社保号码长度为1-50");

            RuleFor(m => m.EnglishName)
                .Length(1, 50).When(m => !string.IsNullOrWhiteSpace(m.EnglishName)).WithMessage("英文名长度为1-50");

            RuleFor(m => m.Company)
                .Length(1, 100).When(m => !string.IsNullOrWhiteSpace(m.Company)).WithMessage("公司名称长度为1-100");

            RuleFor(m => m.TechnicalLevel)
                .Length(1, 50).When(m => !string.IsNullOrWhiteSpace(m.TechnicalLevel)).WithMessage("技术等级长度为1-50");

            RuleFor(m => m.Address)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.Address)).WithMessage("通讯地址长度为1-1024");

            RuleFor(m => m.Remark)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.Remark)).WithMessage("备注长度为1-1024");
        }
    }
}
