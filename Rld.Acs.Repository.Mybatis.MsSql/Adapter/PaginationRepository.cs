using System;
using System.CodeDom;
using System.Collections;
using System.Data;
using Rld.Acs.Model;
using Rld.Acs.Repository.Framework.Pagination;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public abstract class PaginationRepository<TEntity, TKey> : MyBatisRepository<TEntity, TKey>, IPaginationRepository<TEntity, TKey>
        where TEntity : class
    {
        protected string QueryPageStatement { get { return string.Format("{0}.QueryPage", EntityCode); } }

        public PaginationResult<TEntity> QueryPage(Hashtable conditions)
        {
            Int32 totalCount = _sqlMapper.QueryForObject<Int32>(QueryCountStatement, conditions);
            var entities = _sqlMapper.QueryForList<TEntity>(QueryPageStatement, conditions);
            return new PaginationResult<TEntity>()
            {
                TotalCount = totalCount,
                Entities = entities,
            };
        }
    }
}