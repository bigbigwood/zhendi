using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Repository.Framework.Pagination
{
    public class PaginationResult<TEntity> 
        where TEntity : class

    {
        public Int32 TotalCount { get; set; }
        public Int32 CurrentPage { get; set; }
        public Int32 PageSize { get; set; }
        public IEnumerable<TEntity> Entities { get; set; } 
    }
}
