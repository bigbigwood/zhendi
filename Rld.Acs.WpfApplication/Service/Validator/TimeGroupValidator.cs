using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class TimeGroupValidator : AbstractValidator<TimeGroup> 
    {
        public TimeGroupValidator()
        {
            RuleFor(m => m.TimeGroupName)
                .NotEmpty().WithMessage("时间组名称不能为空")
                .Length(1, 50).When(m => !string.IsNullOrWhiteSpace(m.TimeGroupName)).WithMessage("时间组名称不能为空名称为1-50")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("时间组名称不能有特殊字符");

        }
    }
}
