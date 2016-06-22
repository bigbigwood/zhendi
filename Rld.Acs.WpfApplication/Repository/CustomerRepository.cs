using Rld.Acs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Repository
{
    public class CustomerRepository 
    {
        private readonly string BASE_ADDRESS = @"http://localhost:7362";
        public virtual List<Customer> GetAll()
        {
            using (var httpClient = new HttpClient() {BaseAddress = new Uri(BASE_ADDRESS)})
            {
                var response = httpClient.GetAsync("/api/customers").Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
                var customers = response.Content.ReadAsAsync<List<Customer>>().Result;
                return customers;
            }
        }

        public virtual void Delete(Int64 key)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                var response = httpClient.DeleteAsync(string.Format("/api/customers/{0}", key)).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
            }
        }

        public virtual void Update(Customer customer)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                var response = httpClient.PutAsync<Customer>(string.Format("/api/customers/{0}", customer.CustomerId), customer, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
            }
        }

        public virtual void Insert(Customer customer)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                var response = httpClient.PostAsync<Customer>("/api/customers", customer, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
            }
        }
    }
}
