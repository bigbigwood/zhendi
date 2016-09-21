using System;
using System.Linq;
using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class DeviceRoleViewModelValidator : AbstractValidator<DeviceRoleViewModel> 
    {
        public DeviceRoleViewModelValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("设备角色名称不能为空")
                .Must(m => !ValidatorToolkit.HasSpecialChar(m)).WithMessage("设备角色名称不能有特殊字符");

            RuleFor(m => m.DeviceDtos)
                .Must(m => m.Any(x => x.IsSelected)).WithMessage("必须至少关联一台设备");

            RuleFor(m => m.SelectedPermissionAction)
                .Must(m => Enum.GetValues(typeof(DevicePermissionAction)).AsQueryable().Cast<int>().Contains(m)).WithMessage("关联设备权限不能为空");

            RuleFor(m => m.SelectedTimezone)
                .NotEmpty().WithMessage("关联时间区不能为空");
        }
    }
}
