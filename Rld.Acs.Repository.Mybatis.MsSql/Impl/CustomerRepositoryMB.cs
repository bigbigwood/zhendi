using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class CustomerRepositoryMB : MyBatisRepository<Customer, int>, ICustomerRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "Customer.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "Customer.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "Customer.Delete"; }
        }

        protected override string GetByKeyStatement
        {
            get { return "Customer.GetByKey"; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryStatement
        {
            get { return "Customer.Query"; }
        }
        #endregion
    }
}