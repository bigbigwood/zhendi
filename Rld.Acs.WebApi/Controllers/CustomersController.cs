using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rld.Acs.WebApi.Controllers
{
    public class CustomersController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public IQueryable<Customer> GetAllCustomers() 
        //{
        //    using (var conn = RepositoryManager.GetNewConnection())
        //    {
        //        var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
        //        var customer1 = customerRepo.GetByKey(1);
        //    }
        //}

        public Customer GetById(int id) 
        {
            using (var conn = RepositoryManager.GetNewConnection())
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                var customer = customerRepo.GetByKey(id);
                return customer;
            }
        }
    }
}
