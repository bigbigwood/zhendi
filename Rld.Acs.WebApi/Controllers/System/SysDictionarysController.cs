using System.Collections;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.WebApi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Rld.Acs.WebApi.Controllers
{
    public class SysDictionarysController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysDictionaryRepository>();

                IEnumerable<SysDictionary> sysDictionaryInfos;
                if (conditions.ContainsKey(ConstStrings.PageStart) && conditions.ContainsKey(ConstStrings.PageEnd))
                {
                    var paginationResult = repo.QueryPage(conditions);
                    var totalCount = paginationResult.TotalCount;
                    sysDictionaryInfos = paginationResult.Entities;
                    System.Web.HttpContext.Current.Response.Headers.Add(ConstStrings.HTTP_HEADER_X_Pagination_TotalCount, totalCount.ToString());
                }
                else
                {
                    sysDictionaryInfos = repo.Query(conditions);
                }

                return Request.CreateResponse(HttpStatusCode.OK, sysDictionaryInfos.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysDictionaryRepository>();
                var sysDictionaryInfo = repo.GetByKey(id);

                if (sysDictionaryInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, sysDictionaryInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]SysDictionary sysDictionaryInfo)
        {
            return ActionWarpper.Process(sysDictionaryInfo, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysDictionaryRepository>();
                repo.Insert(sysDictionaryInfo);

                return Request.CreateResponse(HttpStatusCode.OK, sysDictionaryInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]SysDictionary sysDictionaryInfo)
        {
            return ActionWarpper.Process(sysDictionaryInfo, new Func<HttpResponseMessage>(() =>
            {
                sysDictionaryInfo.DictionaryID = id;
                var repo = RepositoryManager.GetRepository<ISysDictionaryRepository>();
                repo.Update(sysDictionaryInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<ISysDictionaryRepository>();
                repo.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
