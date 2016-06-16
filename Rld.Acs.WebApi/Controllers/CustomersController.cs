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
        public HttpResponseMessage GetById(int id)
        {
            return Processor.webHandle(id, new Func<HttpResponseMessage>(() =>
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
            return Processor.webHandle(customerDto, new Func<HttpResponseMessage>(() =>
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                customerRepo.Insert(customerDto);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        // PUT api/customers/5
        public HttpResponseMessage Put(int id, [FromBody]Customer customerDto)
        {
            return Processor.webHandle(customerDto, new Func<HttpResponseMessage>(() =>
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
            return Processor.webHandle(id, new Func<HttpResponseMessage>(() =>
            {
                var customerRepo = RepositoryManager.GetRepository<ICustomerRepository>();
                customerRepo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
