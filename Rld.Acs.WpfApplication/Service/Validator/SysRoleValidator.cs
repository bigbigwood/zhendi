using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class SysRoleValidator: AbstractValidator<SysRole> 
    {
        public SysRoleValidator()
        {
            RuleFor(m => m.RoleName)
                .NotEmpty().WithMessage("角色名不能为空");
            
            RuleFor(m => m.Description).Length(1,200).When(m => !string.IsNullOrWhiteSpace(m.Description))
                .WithMessage("描述长度为1-200");
        }
    }
}
