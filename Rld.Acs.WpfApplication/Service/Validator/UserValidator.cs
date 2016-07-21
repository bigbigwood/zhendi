using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class UserValidator: AbstractValidator<User> 
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).NotEmpty();
            RuleFor(user => user.UserCode).NotEmpty();
            RuleFor(m => m.UserPropertyInfo).SetValidator(NinjectBinder.GetValidator<UserPropertyInfoValidator>());
        }
    }
}
