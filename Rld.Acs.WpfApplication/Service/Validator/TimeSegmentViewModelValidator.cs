using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class TimeSegmentViewModelValidator : AbstractValidator<TimeSegmentViewModel>
    {
        public TimeSegmentViewModelValidator()
        {
            RuleFor(m => m.TimeSegmentName)
                .NotEmpty().WithMessage("时间段名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("时间段名称不能有特殊字符")
                .Length(1, 100).WithMessage("时间段名称长度为1-100");
        }
    }
}
