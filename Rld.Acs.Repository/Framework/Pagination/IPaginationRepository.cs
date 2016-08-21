using System.Collections;
using System.Collections.Generic;

namespace Rld.Acs.Repository.Framework.Pagination
{
    public interface IPaginationRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : class
    {
        PaginationResult<TEntity> QueryPage(Hashtable conditions);
    }
}