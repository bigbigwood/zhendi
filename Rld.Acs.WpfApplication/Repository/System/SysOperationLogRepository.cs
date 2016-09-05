using System.Collections;
using Rld.Acs.Model;
using Rld.Acs.Repository.Framework.Pagination;
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
    public class SysOperationLogRepository : BaseRepository<SysOperationLog, int>, ISysOperationLogRepository
    {
        public SysOperationLogRepository()
        {
            RelevantUri = "/api/SysOperationLogs";
        }

        public override bool Update(SysOperationLog log)
        {
            return Update(log, log.LogID);
        }
    }
}
