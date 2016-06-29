using System;
using System.Collections;
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
using GalaSoft.MvvmLight.Messaging;
using Rld.Acs.WpfApplication.Messages;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.ViewModel
{
    public class AllCustomersViewModel : ViewModelBase
    {
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand ModifyCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public CustomerViewModel SelectedCustomerVM { get; private set; }

        private ObservableCollection<CustomerViewModel> _allCustomers = new ObservableCollection<CustomerViewModel>();
        private ICustomerRepository _customerRepository = NinjectBinder.GetRepository<ICustomerRepository>();

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

            Refresh();
        }

        public void Refresh()
        {
            var getAllCustomers = new ObservableCollection<CustomerViewModel>();
            var customers = _customerRepository.Query(new Hashtable()).ToList();
            customers.ForEach(c => getAllCustomers.Add(new CustomerViewModel() { Customer = c}));

            AllCustomers = getAllCustomers;
        }

        public bool HasSelectedCustomer()
        {
            return SelectedCustomerVM != null ? true : false;
        }

        public void AddCustomer()
        {
            Messenger.Default.Send<OpenWindowMessage>(new OpenWindowMessage() {DataContext = new CustomerViewModel() }, Tokens.OpenCustomerView);

            Refresh();
        }

        public void ModifyCustomer()
        {
            Messenger.Default.Send<OpenWindowMessage>(new OpenWindowMessage() { DataContext = SelectedCustomerVM }, Tokens.OpenCustomerView);

            Refresh();
        }

        public void DeleteCustomer()
        {
            _customerRepository.Delete(SelectedCustomerVM.Id);

            Refresh();
        }
    }
}
