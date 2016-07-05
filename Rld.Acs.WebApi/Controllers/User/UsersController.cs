﻿using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.WebApi.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Rld.Acs.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserRepository>();
                var users = repo.Query(conditions);

                return Request.CreateResponse(HttpStatusCode.OK, users.ToList());

            }), this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var repo = RepositoryManager.GetRepository<IUserRepository>();
                var userInfo = repo.GetByKey(id);

                if (userInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, userInfo);

            }), this);
        }

        public HttpResponseMessage Post([FromBody]User userInfo)
        {
            return ActionWarpper.Process(userInfo, new Func<HttpResponseMessage>(() =>
            {
                if (userInfo.UserAuthentications == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "UserAuthenticationInfo property cannot be null.");

                if (userInfo.UserPropertyInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "UserPropertyInfo property cannot be null.");

                var userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                var userPropertyRepo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                var userRepo = RepositoryManager.GetRepository<IUserRepository>();

                userInfo.UserAuthentications.ForEach(a => userAuthenticationRepo.Insert(a));
                userPropertyRepo.Insert(userInfo.UserPropertyInfo);
                userRepo.Insert(userInfo);

                return Request.CreateResponse(HttpStatusCode.OK, userInfo);

            }), this);
        }

        public HttpResponseMessage Put(int id, [FromBody]User userInfo)
        {
            return ActionWarpper.Process(userInfo, new Func<HttpResponseMessage>(() =>
            {
                userInfo.UserID = id;

                var userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                var userPropertyRepo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                var userRepo = RepositoryManager.GetRepository<IUserRepository>();

                var tryFindUserInfo = userRepo.GetByKey(id);
                if (tryFindUserInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("User Id={0} does not exist.", id));

                if (userInfo.UserAuthentications != null)
                {
                    userInfo.UserAuthentications.ForEach(a => userAuthenticationRepo.SaveOrUpdate(a));
                }
                if (userInfo.UserPropertyInfo != null)
                {
                    userPropertyRepo.Update(userInfo.UserPropertyInfo);
                }
                userRepo.Update(userInfo);

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }

        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, new Func<HttpResponseMessage>(() =>
            {
                var userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                var userPropertyRepo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                var userRepo = RepositoryManager.GetRepository<IUserRepository>();

                var userInfo = userRepo.GetByKey(id);
                if (userInfo != null)
                {
                    userInfo.UserAuthentications.ForEach(a => userAuthenticationRepo.Delete(a.UserAuthenticationID));
                    userPropertyRepo.Delete(userInfo.UserPropertyInfo.UserPropertyID);
                    userRepo.Delete(id);
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }), this);
        }
    }
}
