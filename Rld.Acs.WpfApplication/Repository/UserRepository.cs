using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Repository
{
    public class UserRepository : BaseRepository<User, int>, IUserRepository
    {
        public UserRepository()
        {
            RelevantUri = "/api/Users";
        }

        public override bool Update(User user)
        {
            return Update(user, user.UserID);
        }

        public IEnumerable<User> GetDepartmentSummaryUsers(Int32 departmentId)
        {
            using (var httpClient = new HttpClient() {BaseAddress = new Uri(BASE_ADDRESS)})
            {
                string queryString = string.Format("{0}?DepartmentID={1}", RelevantUri, departmentId);

                var response = httpClient.GetAsync(queryString).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
                var entities = response.Content.ReadAsAsync<IEnumerable<User>>().Result;
                return entities;
            }
        }
    }
}
