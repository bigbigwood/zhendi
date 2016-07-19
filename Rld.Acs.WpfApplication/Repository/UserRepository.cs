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
    }
}
