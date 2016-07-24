using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class TimeZoneValidator : AbstractValidator<TimeZone> 
    {
        public TimeZoneValidator()
        {
            RuleFor(m => m.TimeZoneName)
                .NotEmpty().WithMessage("时间区名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("时间区名称不能有特殊字符");

        }
    }
}
