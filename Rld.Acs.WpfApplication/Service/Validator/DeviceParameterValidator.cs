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
        }
    }
}
