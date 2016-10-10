using FluentValidation;
using NPOI.SS.Formula.Functions;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class IcCardAuthenticationViewModelValidator : AbstractValidator<UserAuthenticationViewModel> 
    {
        public IcCardAuthenticationViewModelValidator()
        {
            RuleFor(m => m.AuthenticationData)
                .NotEmpty().WithMessage("已启用的IC卡号码不能为空").When(x => x.IsSelected)
                .Must(ValidatorToolkit.IsNumeric).WithMessage("IC卡号码必须为1-25位数字").When(x => x.IsSelected)
                .Length(1, 25).WithMessage("IC卡号码必须为1-25位数字").When(x => x.IsSelected);
        }
    }
}
