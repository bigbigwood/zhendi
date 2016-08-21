using System.Collections;
using System.Configuration;
using System.Linq;
using Rld.Acs.Repository.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;

namespace Rld.Acs.WpfApplication.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        protected readonly string BASE_ADDRESS = AppConfiguration.BaseServer;
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

        public virtual IEnumerable<TEntity> Query(Hashtable conditions, out int totalCount)
        {
            totalCount = 0;
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                string queryString = "";
                if (conditions.Count > 0)
                {
                    queryString += "?";
                    foreach (DictionaryEntry c in conditions)
                    {
                        queryString += string.Format("{0}={1}&", c.Key, c.Value);
                    }

                    if (queryString.EndsWith("&"))
                        queryString = queryString.Remove(queryString.Length - 1);
                }

                var response = httpClient.GetAsync(RelevantUri + queryString).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code.     
                var entities = response.Content.ReadAsAsync<IEnumerable<TEntity>>().Result;
                totalCount = response.Headers.GetValues(ConstStrings.HTTP_HEADER_X_Pagination_TotalCount).First().ToInt32();
                return entities;
            }
        }

        public virtual IEnumerable<TEntity> Query(Hashtable conditions)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                string queryString = "";
                if (conditions.Count > 0)
                {
                    queryString += "?";
                    foreach (DictionaryEntry c in conditions)
                    {
                        queryString += string.Format("{0}={1}&", c.Key, c.Value);
                    }

                    if (queryString.EndsWith("&"))
                        queryString = queryString.Remove(queryString.Length - 1);
                }

                var response = httpClient.GetAsync(RelevantUri + queryString).Result;
                response.EnsureSuccessStatusCode(); // Throw on error code.     
                var entities = response.Content.ReadAsAsync<IEnumerable<TEntity>>().Result;
                return entities;
            }
        }


        public Int32 QueryCount(Hashtable conditions)
        {
            throw new NotImplementedException();
        }
    }
}
