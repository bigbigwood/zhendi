using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class TimeZoneViewModelValidator : AbstractValidator<TimeZoneViewModel> 
    {
        public TimeZoneViewModelValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("时间区名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("时间区名称不能有特殊字符")
                .Length(1, 50).WithMessage("时间区名称长度为1-50");

            RuleFor(m => m.Code)
                .NotEmpty().WithMessage("时间区编号不能为空")
                .Must(ValidatorToolkit.IsNumeric).WithMessage("时间区编号必须为数字");
        }
    }
}
