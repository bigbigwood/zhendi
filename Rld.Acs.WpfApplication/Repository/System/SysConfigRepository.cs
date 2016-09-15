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
    public class SysConfigRepository : CacheableRepository<SysConfig, int>, ISysConfigRepository
    {
        public SysConfigRepository()
        {
            RelevantUri = "/api/SysConfigs";
            CacheKey = "CacheKey_SysConfigs";
            CacheExpireMinutes = SystemCacheExpireMinutes;
        }

        public override bool Update(SysConfig sysConfig)
        {
            return Update(sysConfig, sysConfig.ID);
        }

        public override SysConfig GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.ID == key);
        }

        public String GetConfigValueByName(string name)
        {
            throw  new NotImplementedException();
        }
    }
}
