﻿using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class UserRepositoryMB : MyBatisRepository<User, int>, IUserRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "User"; }
        }
        #endregion
    }
}