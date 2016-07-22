using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class UserValidator: AbstractValidator<User> 
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage("人员名称不能为空");
            RuleFor(user => user.UserCode).NotEmpty().WithMessage("人员编号不能为空");
            RuleFor(m => m.UserPropertyInfo).SetValidator(NinjectBinder.GetValidator<UserPropertyInfoValidator>());
        }
    }
}
