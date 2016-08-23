using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class SysDictionaryRepositoryMB : PaginationRepository<SysDictionary, int>, ISysDictionaryRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "SysDictionary"; }
        }
        #endregion
    }
}