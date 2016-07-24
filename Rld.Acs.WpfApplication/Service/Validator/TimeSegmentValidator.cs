using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class TimeSegmentValidator : AbstractValidator<TimeSegment> 
    {
        public TimeSegmentValidator()
        {
            RuleFor(m => m.TimeSegmentName)
                .NotEmpty().WithMessage("时间段名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("时间段名称不能有特殊字符");

            RuleFor(m => m.BeginTime)
                .NotEmpty().WithMessage("开始时间不能为空")
                .Must(p => ValidatorToolkit.IsTimeSegmentFormat(p) && p.Length == 5).WithMessage("开始时间格式错误，必须是NN：NN格式, 例如09:30");

            RuleFor(m => m.EndTime)
                .NotEmpty().WithMessage("结束时间不能为空")
                .Must(p => ValidatorToolkit.IsTimeSegmentFormat(p) && p.Length == 5).WithMessage("结束时间格式错误，必须是NN：NN格式, 例如09:30");

        }
    }
}
