using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class FloorValidator: AbstractValidator<Floor> 
    {
        public FloorValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("楼层名称不能为空")
                .Length(1, 100).WithMessage("楼层名称长度为1-100")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("楼层名称不能有特殊字符");

            RuleFor(m => m.Photo)
                .NotEmpty().WithMessage("请上传楼层平面图");
        }
    }
}
