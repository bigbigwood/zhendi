using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class SysOperatorValidator: AbstractValidator<SysOperator> 
    {
        public SysOperatorValidator()
        {
            RuleFor(m => m.LoginName)
                .NotEmpty().WithMessage("登录名不能为空");
            
            RuleFor(m => m.LanguageID)
                .NotEmpty().WithMessage("语言不能为空");
        }
    }
}
