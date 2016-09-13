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
    public class DepartmentRepository : CacheableRepository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository()
        {
            RelevantUri = "/api/Departments";
            CacheKey = "CacheKey_Departments";
            CacheExpireMinutes = DepartmentCacheExpireMinutes;
        }

        public override bool Update(Department department)
        {
            return Update(department, department.DepartmentID);
        }

        public override Department GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.DepartmentID == key);
        }
    }
}
