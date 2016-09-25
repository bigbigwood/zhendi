using System.Collections;
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

        public IEnumerable<User> QueryUsersForSummaryData(Hashtable conditions)
        {
            var uri = "/api/UserSummarys";
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                string queryString = "";
                if (conditions.Count > 0)
                {
                    queryString += "?";
                    foreach (DictionaryEntry c in conditions)
                    {
                        queryString += string.Format("{0}={1}&", c.Key, c.Value);
                    }

                    if (queryString.EndsWith("&"))
                        queryString = queryString.Remove(queryString.Length - 1);
                }

                var response = httpClient.GetAsync(uri + queryString).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code.     
                var entities = response.Content.ReadAsAsync<IEnumerable<User>>().Result;
                return entities;
            }
        }

        public Int32 QueryUsersCount(Hashtable conditions)
        {
            var uri = "/api/UserCounts";
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                string queryString = "";
                if (conditions.Count > 0)
                {
                    queryString += "?";
                    foreach (DictionaryEntry c in conditions)
                    {
                        queryString += string.Format("{0}={1}&", c.Key, c.Value);
                    }

                    if (queryString.EndsWith("&"))
                        queryString = queryString.Remove(queryString.Length - 1);
                }

                var response = httpClient.GetAsync(uri + queryString).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code.     
                var count = response.Content.ReadAsAsync<Int32>().Result;
                return count;
            }
        }
    }
}
