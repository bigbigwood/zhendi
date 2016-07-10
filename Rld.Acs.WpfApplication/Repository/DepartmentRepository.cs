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
    public class DepartmentRepository : BaseRepository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository()
        {
            RelevantUri = "/api/Departments";
        }

        public override bool Update(Department department)
        {
            return Update(department, department.DepartmentID);
        }
    }
}
