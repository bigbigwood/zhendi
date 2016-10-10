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
            RuleFor(m => m.TimeSegmentCode)
                .NotEmpty().WithMessage("时间段编号不能为空")
                .Must(ValidatorToolkit.IsNumeric).WithMessage("时间段编号必须为数字");
            RuleFor(m => m.StartHour)
                .NotEmpty().WithMessage("开始时间小时数值不能为空")
                .Must(ValidatorToolkit.VerifyHourFormat).WithMessage("开始时间小时数值无效");
            RuleFor(m => m.EndHour)
                .NotEmpty().WithMessage("结束时间小时数值不能为空")
                .Must(ValidatorToolkit.VerifyHourFormat).WithMessage("结束时间小时数值无效");
            RuleFor(m => m.StartMinute)
                .NotEmpty().WithMessage("开始时间分钟数值不能为空")
                .Must(ValidatorToolkit.VerifyMinuteFormat).WithMessage("开始时间分钟数值无效");
            RuleFor(m => m.EndMinute)
                .NotEmpty().WithMessage("结束时间分钟数值不能为空")
                .Must(ValidatorToolkit.VerifyMinuteFormat).WithMessage("结束时间分钟数值无效");
        }
    }
}
