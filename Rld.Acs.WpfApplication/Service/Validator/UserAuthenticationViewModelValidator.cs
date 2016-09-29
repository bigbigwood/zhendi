using FluentValidation;
using NPOI.SS.Formula.Functions;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class UserAuthenticationViewModelValidator : AbstractValidator<UserAuthenticationViewModel> 
    {
        public UserAuthenticationViewModelValidator()
        {
            RuleFor(m => m.AuthenticationData)
                .NotEmpty().WithMessage("已启用的人员密码不能为空").When(x => x.IsSelected)
                .Must(ValidatorToolkit.IsNumeric).WithMessage("密码必须为1-8位数字").When(x => x.IsSelected)
                .Length(1, 8).WithMessage("密码必须为1-8位数字").When(x => x.IsSelected);
        }
    }
}
