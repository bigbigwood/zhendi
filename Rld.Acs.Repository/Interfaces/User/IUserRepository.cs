using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Rld.Acs.Repository.Framework;
using Rld.Acs.Model;

namespace Rld.Acs.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User, Int32>
    {
        IEnumerable<User> QueryUsersForSummaryData(Hashtable conditions);

        Int32 QueryUsersCount(Hashtable conditions);
    }
}