using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class TimeGroupViewModelValidator : AbstractValidator<TimeGroupViewModel> 
    {
        public TimeGroupViewModelValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("时间组名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("时间组名称不能有特殊字符")
                .Length(1, 50).WithMessage("时间组名称长度为1-50"); 

        }
    }
}
