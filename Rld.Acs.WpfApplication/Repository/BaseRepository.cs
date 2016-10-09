using System.Collections;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Rld.Acs.Repository.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using Rld.Acs.Repository.Framework.Pagination;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Exceptions;
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
                httpClient.DefaultRequestHeaders.Authorization = BuildAuthHeader();
                var response = httpClient.PostAsync(RelevantUri, entity, new JsonMediaTypeFormatter()).Result;
                if (response.IsSuccessStatusCode)
                {
                    var newEntity = response.Content.ReadAsAsync<TEntity>().Result;
                    return newEntity;
                }

                var errorMessage = response.Content.ReadAsStringAsync().Result;
                if (response.ReasonPhrase == ConstStrings.BusinessLogicError)
                    throw new BusinessException(errorMessage);
                else
                    throw new Exception(errorMessage);
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
                httpClient.DefaultRequestHeaders.Authorization = BuildAuthHeader();
                var response = httpClient.PutAsync(string.Format("{0}/{1}", RelevantUri, key), entity, new JsonMediaTypeFormatter()).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMessage = response.Content.ReadAsStringAsync().Result;
                if (response.ReasonPhrase == ConstStrings.BusinessLogicError)
                    throw new BusinessException(errorMessage);
                else
                    throw new Exception(errorMessage);
            }
        }

        public virtual bool Delete(TKey key)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) })
            {
                httpClient.DefaultRequestHeaders.Authorization = BuildAuthHeader();
                var response = httpClient.DeleteAsync(string.Format("{0}/{1}", RelevantUri, key)).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMessage = response.Content.ReadAsStringAsync().Result;
                if (response.ReasonPhrase == ConstStrings.BusinessLogicError)
                    throw new BusinessException(errorMessage);
                else
                    throw new Exception(errorMessage);
            }
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

        public virtual PaginationResult<TEntity> QueryPage(Hashtable conditions)
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
                int totalCount = response.Headers.GetValues(ConstStrings.HTTP_HEADER_X_Pagination_TotalCount).First().ToInt32();

                return new PaginationResult<TEntity>()
                {
                    TotalCount = totalCount,
                    Entities = entities,
                };
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

        private AuthenticationHeaderValue BuildAuthHeader()
        {
            var username = ApplicationManager.GetInstance().CurrentOperatorInfo.LoginName;
            var password = ApplicationManager.GetInstance().CurrentOperatorInfo.Password;

            var buffer = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password));
            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));
            return authHeader;
        }
    }
}
