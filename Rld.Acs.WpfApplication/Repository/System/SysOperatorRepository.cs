using System;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.Repository.System
{
    public class SysOperatorRepository : BaseRepository<SysOperator, int>, ISysOperatorRepository
    {
        public SysOperatorRepository()
        {
            RelevantUri = "/api/SysOperators";
        }

        public override bool Update(SysOperator sysOperator)
        {
            return Update(sysOperator, sysOperator.OperatorID);
        }

        public bool UpdatePassword(SysOperator sysOperatorInfo)
        {
            throw new NotImplementedException();
        }
    }
}
