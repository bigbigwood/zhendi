using System.Collections;
using Rld.Acs.Repository.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Rld.Acs.WpfApplication.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly string BASE_ADDRESS = @"http://localhost:7362";
        protected string RelevantUri { get; set; }

        public virtual TEntity Insert(TEntity entity)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                var response = httpClient.PostAsync<TEntity>(RelevantUri, entity, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
                var newEntity = response.Content.ReadAsAsync<TEntity>().Result;
                return newEntity;
            }
        }

        public virtual bool Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual bool Update(TEntity entity, TKey key)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                var response = httpClient.PutAsync<TEntity>(string.Format("{0}/{1}", RelevantUri, key), entity, new JsonMediaTypeFormatter()).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
                return true;
            }

            return false;
        }

        public virtual bool Delete(TKey key)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                var response = httpClient.DeleteAsync(string.Format("{0}/{1}", RelevantUri, key)).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
                return true;
            }
            return false;
        }

        public virtual TEntity GetByKey(TKey key)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                var response = httpClient.GetAsync(string.Format("{0}/{1}", RelevantUri, key)).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
                var entity = response.Content.ReadAsAsync<TEntity>().Result;
                return entity;
            }
        }

        public virtual IEnumerable<TEntity> Query(Hashtable conditions)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                var response = httpClient.GetAsync(RelevantUri).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code. 
                var entities = response.Content.ReadAsAsync<IEnumerable<TEntity>>().Result;
                return entities;
            }
        }
    }
}
