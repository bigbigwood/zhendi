using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class DeviceValidator: AbstractValidator<DeviceController> 
    {
        public DeviceValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("设备名称不能为空")
                .Length(1, 100).WithMessage("设备名称长度为1-100")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("设备名称不能有特殊字符");
            
            RuleFor(m => m.Code)
                .NotEmpty().WithMessage("设备编号不能为空")
                .Must(ValidatorToolkit.IsNumeric).WithMessage("设备编号只能为数字");

            RuleFor(m => m.Model)
                .NotEmpty().WithMessage("设备型号不能为空")
                .Length(1, 100).WithMessage("设备型号长度为1-100");

            RuleFor(m => m.Mac)
                .NotEmpty().WithMessage("Mac地址不能为空")
                .Length(1, 100).WithMessage("Mac地址长度为1-100");

            RuleFor(m => m.SN)
                .NotEmpty().WithMessage("产品序列号不能为空")
                .Length(1, 100).WithMessage("产品序列号长度为1-100");

            RuleFor(m => m.BaudRate)
                .Length(1, 100).When(m => !string.IsNullOrWhiteSpace(m.BaudRate)).WithMessage("波特率长度为1-100");
            
            RuleFor(m => m.SerialPort)
                .Length(1, 100).When(m => !string.IsNullOrWhiteSpace(m.SerialPort)).WithMessage("串号端口号长度为1-100");
            
            RuleFor(m => m.Password)
                .Length(1, 100).When(m => !string.IsNullOrWhiteSpace(m.Password)).WithMessage("密码长度为1-100");
            
            RuleFor(m => m.IP)
                .Length(1, 100).When(m => !string.IsNullOrWhiteSpace(m.IP)).WithMessage("设备Ip长度为1-100");
            
            RuleFor(m => m.Port)
                .Must(ValidatorToolkit.IsNumeric).When(m => !string.IsNullOrWhiteSpace(m.Port)).WithMessage("设备端口只能为数字");
            
            RuleFor(m => m.ServerURL)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.ServerURL)).WithMessage("后台服务器地址长度为1-1024");
            
            RuleFor(m => m.Label)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.Label)).WithMessage("备注长度为1-1024");
            
            RuleFor(m => m.Remark)
                .Length(1, 1024).When(m => !string.IsNullOrWhiteSpace(m.Remark)).WithMessage("备注长度为1-1024");

            RuleFor(m => m.DeviceControllerParameter).SetValidator(NinjectBinder.GetValidator<DeviceParameterValidator>());

            RuleFor(m => m.DeviceDoors).SetCollectionValidator(NinjectBinder.GetValidator<DoorValidator>());

            RuleFor(m => m.DeviceHeadReadings).SetCollectionValidator(NinjectBinder.GetValidator<HeadReadingValidator>());
        }
    }
}
