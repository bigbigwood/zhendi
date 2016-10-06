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
    public interface IUserEventRepository : IRepository<UserEvent, Int64>
    {
    }
}