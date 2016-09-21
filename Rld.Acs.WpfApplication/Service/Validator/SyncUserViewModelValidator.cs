using System.Data;
using System.Linq;
using FluentValidation;
using Rld.Acs.Model;
using Rld.Acs.Unility;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.Service.Validator
{
    public class SyncUserViewModelValidator: AbstractValidator<SyncUserViewModel> 
    {
        public SyncUserViewModelValidator()
        {
            RuleFor(m => m.DeviceDtos).Must(m => m.Any(d => d.IsSelected)).WithMessage("同步设备未选择！");
            RuleFor(m => m.SelectedSyncUserDtos).Must(m => m.Any()).WithMessage("同步人员未选择！");
        }
    }
}
