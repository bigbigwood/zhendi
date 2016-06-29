using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class DepartmentRepositoryMB : MyBatisRepository<Department, int>, IDepartmentRepository
    {
        #region Repository
        protected override string InsertStatement
        {
            get { return "Department.Insert"; }
        }

        protected override string UpdateStatement
        {
            get { return "Department.Update"; }
        }

        protected override string DeleteStatement
        {
            get { return "Department.Delete"; }
        }

        protected override string GetByKeyStatement
        {
            get { return "Department.GetByKey"; }
        }

        protected override string QueryCountStatement
        {
            get { return null; }
        }

        protected override string QueryStatement
        {
            get { return "Department.Query"; }
        }
        #endregion
    }
}