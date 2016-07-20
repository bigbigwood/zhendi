using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Validator
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
