using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.Acs.Repository.Framework
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>  
        /// 新增  
        /// </summary>  
        /// <param name="entity"></param>  
        /// <returns></returns>  
        TEntity Insert(TEntity entity);
        /// <summary>  
        /// 编辑  
        /// </summary>  
        /// <param name="entity"></param>  
        /// <returns></returns>  
        bool Update(TEntity entity);
        /// <summary>  
        /// 删除  
        /// </summary>  
        /// <param name="entity"></param>  
        /// <returns></returns>  
        bool Delete(TKey key);
        /// <summary>  
        /// 单个查询  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        TEntity GetByKey(TKey key);
        /// <summary>  
        /// 模糊查询  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        IEnumerable<TEntity> Query(Hashtable conditions);
    }  
}