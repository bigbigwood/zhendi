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
using System.Web.Http.Results;

namespace Rld.Acs.WebApi.Controllers
{
    public class CustomersController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET api/customers
        public IEnumerable<Customer> Get()
        {
            //var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();

            //string p1Val = allUrlKeyValues.SingleOrDefault(x => x.Key == "p1").Value;
            //string p2Val = allUrlKeyValues.SingleOrDefault(x => x.Key == "p2").Value;
            //string p3Val = allUrlKeyValues.SingleOrDefault(x => x.Key == "p3").Value;

            //return new string[] { "value1", "value2", p1Val, p2Val, p3Val };

            throw new NotImplementedException();
        }

        // GET api/customers/5
        public IHttpActionResult GetById(int id)
        {
            using (var conn = RepositoryManager.GetNewConnection())
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                var customer = customerRepo.GetByKey(id);
                if (customer == null)
                     return NotFound();

                return Ok(customer);
            }
        }

        // POST api/customers
        public IHttpActionResult Post([FromBody]Customer customerDto)
        {
            using (var conn = RepositoryManager.GetNewConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                    customerRepo.Insert(customerDto);

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


                return Ok();
            }
        }

        // PUT api/customers/5
        public IHttpActionResult Put(int id, [FromBody]Customer customerDto)
        {
            using (var conn = RepositoryManager.GetNewConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();

                    var customer = customerDto;
                    customer.CustomerId = id;
                    customerRepo.Update(customer);

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

                return Ok();
            }
        }

        // DELETE api/customer/5
        public IHttpActionResult Delete(int id)
        {
            using (var conn = RepositoryManager.GetNewConnection())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                    bool isSuccess = customerRepo.Delete(id);

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

                return Ok();
            }

        }
    }
}
