using System;
using System.Collections;
using System.Data;
using System.Linq;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysConfigRepositoryMB : MyBatisRepository<SysConfig, int>, ISysConfigRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysConfig"; }
        }
        #endregion

        public String GetConfigValueByName(string name)
        {
            var config = this.Query(new Hashtable { { "Name", name } }).FirstOrDefault();
            return config == null ? string.Empty : config.Value;
        }
    }
}