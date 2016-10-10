using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class DeviceParameterValidator: AbstractValidator<DeviceControllerParameter> 
    {
        public DeviceParameterValidator()
        {
            RuleFor(m => m.DuressPassword)
                .Length(1, 100).When(m => !string.IsNullOrWhiteSpace(m.DuressPassword)).WithMessage("胁迫密码长度为1-100");
        }
    }
}
