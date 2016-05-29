using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WebService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Rld.Acs.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SampleService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SampleService.svc or SampleService.svc.cs at the Solution Explorer and start debugging.
    public class SampleService : ISample
    {
        public void DoSomething()
        {
            using (var conn = RepositoryManager.GetNewConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                    var customer1 = customerRepo.GetByKey(1);

                    customerRepo.Insert(customer1);

                    customer1.FirstName = "firstName";
                    customer1.LastName = "lastName";
                    customerRepo.Update(customer1);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // transaction rollback
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
