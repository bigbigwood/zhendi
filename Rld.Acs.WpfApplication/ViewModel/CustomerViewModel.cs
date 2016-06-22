using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.WpfApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private Customer _customer = null;
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        private CustomerRepository _customerRepository = new CustomerRepository();

        #region fields

        public Int64 Id
        {
            get { return _customer.CustomerId; }
        }

        public string FirstName
        {
            get { return _customer.FirstName; }
            set
            {
                if (_customer.FirstName != value)
                {
                    _customer.FirstName = value;
                }
            }
        }

        public string LastName
        {
            get { return _customer.LastName; }
            set
            {
                if (_customer.LastName != value)
                {
                    _customer.LastName = value;
                }
            }
        }

        public string MSIDSN
        {
            get { return _customer.MSIDSN; }
            set
            {
                if (_customer.MSIDSN != value)
                {
                    _customer.MSIDSN = value;
                }
            }
        }

        public decimal Balance
        {
            get { return _customer.Balance; }
            set
            {
                if (_customer.Balance != value)
                {
                    _customer.Balance = value;
                }
            }
        }

        public string Address
        {
            get { return _customer.Address; }
            set
            {
                if (_customer.Address != value)
                {
                    _customer.Address = value;
                }
            }
        }

        #endregion

        public CustomerViewModel(Customer customer)
        {
            _customer = customer;
            SaveCommand = new RelayCommand(Save, () => CanSave());
            CloseCommand = new RelayCommand(Close);
        }

        /// <summary>
        /// Saves the customer to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            if (_customer.CustomerId == 0)
            {
                _customer.ResigterDateTime = DateTime.Now;
                _customerRepository.Insert(_customer);
            }
            else
            {
                _customerRepository.Update(_customer);
            }

            Close();
        }

        /// <summary>
        /// Returns true if the customer is valid and can be saved.
        /// </summary>
        public bool CanSave()
        {
            return (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(MSIDSN) && Balance >= 0);
        }

        public void Close()
        {
            Messenger.Default.Send<CustomerViewMessage>(null, "Close");
        }
    }
}
