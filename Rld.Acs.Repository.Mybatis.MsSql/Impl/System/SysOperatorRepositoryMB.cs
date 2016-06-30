﻿using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysOperatorRepositoryMB : MyBatisRepository<SysOperator, int>, ISysOperatorRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysOperator"; }
        }
        #endregion
    }
}