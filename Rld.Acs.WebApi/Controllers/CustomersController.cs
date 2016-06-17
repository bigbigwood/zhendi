using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WebApi.Framework;
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
        public HttpResponseMessage Get()
        {
            var conditions = new Customer();
            var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
            conditions.FirstName = allUrlKeyValues.SingleOrDefault(x => x.Key == "FirstName").Value;
            conditions.LastName = allUrlKeyValues.SingleOrDefault(x => x.Key == "LastName").Value;
            conditions.MSIDSN = allUrlKeyValues.SingleOrDefault(x => x.Key == "MSIDSN").Value;

            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                var customer = customerRepo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, customer.ToList());

            }), this);
        }

        // GET api/customers/5
        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                var customer = customerRepo.GetByKey(id);

                if (customer == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, customer);

            }), this);
        }

        // POST api/customers
        public HttpResponseMessage Post([FromBody]Customer customerDto)
        {
            return ActionWarpper.Process(customerDto, new Func<HttpResponseMessage>(() =>
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                customerRepo.Insert(customerDto);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        // PUT api/customers/5
        public HttpResponseMessage Put(int id, [FromBody]Customer customerDto)
        {
            return ActionWarpper.Process(customerDto, new Func<HttpResponseMessage>(() =>
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();

                var customer = customerDto;
                customer.CustomerId = id;
                customerRepo.Update(customer);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        // DELETE api/customer/5
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                customerRepo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
