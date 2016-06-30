using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class CustomerRepositoryMB : MyBatisRepository<Customer, int>, ICustomerRepository
    {
        #region Repository
        protected override string EntityCode
        {
            get { return "Customer"; }
        }
        #endregion
    }
}