﻿using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysRoleRepositoryMB : MyBatisRepository<SysRole, int>, ISysRoleRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysRole"; }
        }
        #endregion
    }
}