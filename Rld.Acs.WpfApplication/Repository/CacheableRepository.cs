using System.Collections;
using System.Linq;
using System.Reflection;
using log4net;
using Rld.Acs.Repository.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Rld.Acs.WpfApplication.Repository
{
    public abstract class CacheableRepository<TEntity, TKey> : BaseRepository<TEntity, TKey>, IRepository<TEntity, TKey>
        where TEntity : class
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const Int32 SystemCacheExpireMinutes = 60;
        public const Int32 DepartmentCacheExpireMinutes = 10;

        protected String CacheKey { get; set; }
        protected Int32 CacheExpireMinutes { get; set; }
        private static readonly Object _locker = new object();

        public virtual TEntity Insert(TEntity entity)
        {
            var cachedEntities = CacheableQuery();

            entity = base.Insert(entity);

            ObjectCache cache = MemoryCache.Default;
            cachedEntities.Add(entity);
            lock (_locker)
            {
                cache.Set(CacheKey, cachedEntities, GetPolicy());
            }

            return entity;
        }

        public virtual bool Update(TEntity entity, TKey key)
        {
            var cachedEntities = CacheableQuery();

            bool result = base.Update(entity, key);

            if (result)
            {
                ObjectCache cache = MemoryCache.Default;
                var cacheEntity = GetByKey(key);
                var index = cachedEntities.IndexOf(cacheEntity);
                cachedEntities[index] = entity;

                lock (_locker)
                {
                    cache.Set(CacheKey, cachedEntities, GetPolicy());
                }
            }

            return result;
        }

        public virtual bool Delete(TKey key)
        {
            var cachedEntities = CacheableQuery();

            bool result = base.Delete(key);

            if (result)
            {
                ObjectCache cache = MemoryCache.Default;
                var cacheEntity = GetByKey(key);
                cachedEntities.Remove(cacheEntity);

                lock (_locker)
                {
                    cache.Set(CacheKey, cachedEntities, GetPolicy());
                }
            }

            return result;
        }

        public virtual IEnumerable<TEntity> Query(Hashtable conditions)
        {
            return CacheableQuery();
        }

        protected virtual List<TEntity> CacheableQuery()
        {
            ObjectCache cache = MemoryCache.Default;
            List<TEntity> entities;
            if (cache.Contains(CacheKey))
            {
                entities = cache.GetCacheItem(CacheKey).Value as List<TEntity>;
            }
            else
            {
                Log.InfoFormat("Loading data for cacheable repository, cache key= {0}", CacheKey);
                entities = base.Query(new Hashtable()).ToList();
                cache.Set(CacheKey, entities, GetPolicy());
            }

            return entities;
        }

        public virtual void Refresh()
        {
            Log.InfoFormat("Loading data for cacheable repository, cache key= {0}", CacheKey);
            var entities = base.Query(new Hashtable()).ToList();

            ObjectCache cache = MemoryCache.Default;
            lock (_locker)
            {
                if (cache.Contains(CacheKey))
                {
                    Log.InfoFormat("Remove cache data key= {0}", CacheKey);
                    cache.Remove(CacheKey);
                }
                cache.Set(CacheKey, entities, GetPolicy());
            }
        }

        public virtual TEntity Refresh(TKey key)
        {
            var cachedEntities = CacheableQuery();

            var entity = base.GetByKey(key);
            if (entity != null)
            {
                ObjectCache cache = MemoryCache.Default;
                var cacheEntity = GetByKey(key);
                var index = cachedEntities.IndexOf(cacheEntity);
                cachedEntities[index] = entity;

                lock (_locker)
                {
                    cache.Set(CacheKey, cachedEntities, GetPolicy());
                }
            }

            return entity;
        }

        private CacheItemPolicy GetPolicy()
        {
            return new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable, SlidingExpiration = TimeSpan.FromMinutes(CacheExpireMinutes) };
        }
    }
}
