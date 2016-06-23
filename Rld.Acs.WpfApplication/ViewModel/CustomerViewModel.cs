using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
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
        public Customer Customer { get; set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        private ICustomerRepository _customerRepository = NinjectBinder.GetRepository<ICustomerRepository>();

        #region fields

        public int Id
        {
            get { return Customer.CustomerId; }
        }

        public string FirstName
        {
            get { return Customer.FirstName; }
            set { Customer.FirstName = value;}
        }

        public string LastName
        {
            get { return Customer.LastName; }
            set {Customer.LastName = value;}
        }

        public string MSIDSN
        {
            get { return Customer.MSIDSN; }
            set {Customer.MSIDSN = value;}
        }

        public decimal Balance
        {
            get { return Customer.Balance; }
            set {  Customer.Balance = value;}
        }

        public string Address
        {
            get { return Customer.Address; }
            set { Customer.Address = value;}
        }

        #endregion

        public CustomerViewModel()
        {
            Customer = new Customer();
            SaveCommand = new RelayCommand(Save, () => CanSave());
            CloseCommand = new RelayCommand(Close);
        }

        /// <summary>
        /// Saves the customer to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            if (Customer.CustomerId == 0)
            {
                Customer.ResigterDateTime = DateTime.Now;
                _customerRepository.Insert(Customer);
            }
            else
            {
                _customerRepository.Update(Customer);
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
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(Tokens.CloseCustomerView), Tokens.CloseCustomerView);
        }
    }
}
