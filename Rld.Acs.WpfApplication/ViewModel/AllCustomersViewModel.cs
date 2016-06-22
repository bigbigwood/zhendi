using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Rld.Acs.Model;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class AllCustomersViewModel : ViewModelBase
    {
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand ModifyCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public CustomerViewModel SelectedCustomerVM { get; private set; }

        private ObservableCollection<CustomerViewModel> _allCustomers = null;
        private CustomerRepository _customerRepository = new CustomerRepository();

        public ObservableCollection<CustomerViewModel> AllCustomers
        {
            get { return _allCustomers; }
            set
            {
                if (_allCustomers != value)
                {
                    _allCustomers = value;
                    RaisePropertyChanged("AllCustomers");
                }
            }
        }

        public AllCustomersViewModel()
        {
            AddCommand = new RelayCommand(AddCustomer);
            ModifyCommand = new RelayCommand(ModifyCustomer, () => HasSelectedCustomer());
            DeleteCommand = new RelayCommand(DeleteCustomer, () => HasSelectedCustomer());
            _allCustomers = new ObservableCollection<CustomerViewModel>();

            Refresh();
        }

        public void Refresh()
        {
            var getAllCustomers = new ObservableCollection<CustomerViewModel>();
            var customers = _customerRepository.GetAll().ToList();
            customers.ForEach(c => getAllCustomers.Add(new CustomerViewModel(c)));

            AllCustomers = getAllCustomers;
        }

        public bool HasSelectedCustomer()
        {
            return SelectedCustomerVM != null ? true : false;
        }

        public void AddCustomer()
        {
            var customerView = new Pages.CustomerView();
            customerView.DataContext = new CustomerViewModel(new Customer());
            customerView.ShowDialog();

            Refresh();
        }

        public void ModifyCustomer()
        {
            var customerView = new Pages.CustomerView();
            customerView.DataContext = SelectedCustomerVM;
            customerView.ShowDialog();

            Refresh();
        }

        public void DeleteCustomer()
        {
            _customerRepository.Delete(SelectedCustomerVM.Id);

            Refresh();
        }
    }
}
