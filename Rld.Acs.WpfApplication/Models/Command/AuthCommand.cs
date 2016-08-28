using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Rld.Acs.WpfApplication.Service.Authorization;

namespace Rld.Acs.WpfApplication.Models.Command
{
    public class AuthCommand : RelayCommand, ICommand
    {
        public AuthCommand(Action execute)
            :base(execute)
        {
        }

        bool ICommand.CanExecute(object parameter)
        {
            return AuthProvider.Instance.CheckAccess(parameter);
        }
    }
}
