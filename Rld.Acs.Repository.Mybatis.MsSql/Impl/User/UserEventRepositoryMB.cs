using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserEventRepositoryMB : MyBatisRepository<UserEvent, Int64>, IUserEventRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "UserEvent"; }
        }
        #endregion
    }
}