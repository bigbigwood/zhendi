using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Encryption;
using Rld.Acs.Unility.Extension;
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
            return ActionWarpper.Process(conditions, OperationCodes.QUS, () =>
            {
                var repo = RepositoryManager.GetRepository<IUserRepository>();
                var users = repo.Query(conditions);

                users.ForEach(DecodePassword);
                return Request.CreateResponse(HttpStatusCode.OK, users.ToList());
               
            }, this);
        }

        public HttpResponseMessage GetById(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.GUS, () =>
            {
                var repo = RepositoryManager.GetRepository<IUserRepository>();
                var userInfo = repo.GetByKey(id);

                if (userInfo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                DecodePassword(userInfo);
                return Request.CreateResponse(HttpStatusCode.OK, userInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Post([FromBody]User userInfo)
        {
            return ActionWarpper.Process(userInfo, OperationCodes.AUS, () =>
            {
                if (userInfo.UserAuthentications == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "UserAuthenticationInfo property cannot be null.");

                if (userInfo.UserPropertyInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "UserPropertyInfo property cannot be null.");

                if (userInfo.UserDeviceRoles == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "UserDeviceRoles property cannot be null.");

                var userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                var userPropertyRepo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                var userRepo = RepositoryManager.GetRepository<IUserRepository>();
                var userDeviceRoleRepo = RepositoryManager.GetRepository<IUserDeviceRoleRepository>();
                var userEventRepo = RepositoryManager.GetRepository<IUserEventRepository>();

                userPropertyRepo.Insert(userInfo.UserPropertyInfo);
                userRepo.Insert(userInfo);

                EncodePassword(userInfo);
                userInfo.UserAuthentications.ForEach(a => a.UserID = userInfo.UserID);
                userInfo.UserAuthentications.ForEach(a => userAuthenticationRepo.Insert(a));

                userInfo.UserDeviceRoles.ForEach(a => a.UserID = userInfo.UserID);
                userInfo.UserDeviceRoles.ForEach(a => userDeviceRoleRepo.Insert(a));

                userEventRepo.Insert(new UserEvent()
                {
                    EventType = UserEventType.Add,
                    UserID = userInfo.UserID,
                    CreateDate = DateTime.Now,
                    CreateUserID = GlobalSetting.WebApiUserId,
                });

                return Request.CreateResponse(HttpStatusCode.OK, userInfo);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Put(int id, [FromBody]User userInfo)
        {
            return ActionWarpper.Process(userInfo, OperationCodes.MUS, () =>
            {
                userInfo.UserID = id;

                var userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                var userPropertyRepo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                var userRepo = RepositoryManager.GetRepository<IUserRepository>();
                var userDeviceRoleRepo = RepositoryManager.GetRepository<IUserDeviceRoleRepository>();
                var userEventRepo = RepositoryManager.GetRepository<IUserEventRepository>();

                var originalUserInfo = userRepo.GetByKey(id);
                if (originalUserInfo == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("User Id={0} does not exist.", id));

                var addedAuthentications = new List<UserAuthentication>();
                var deletedAuthenticationIds = new List<int>();
                if (userInfo.UserAuthentications != null && userInfo.UserAuthentications.Any())
                {
                    var originalUserAuthenticationIDs = originalUserInfo.UserAuthentications.Select(d => d.UserAuthenticationID);
                    var UserAuthenticationIDs = userInfo.UserAuthentications.Select(d => d.UserAuthenticationID);
                    deletedAuthenticationIds = originalUserAuthenticationIDs.Except(UserAuthenticationIDs).ToList();

                    addedAuthentications = userInfo.UserAuthentications.FindAll(d => d.UserAuthenticationID == 0);
                }
                else
                {
                    deletedAuthenticationIds = originalUserInfo.UserAuthentications.Select(d => d.UserAuthenticationID).ToList();
                }

                EncodePassword(userInfo);
                deletedAuthenticationIds.ForEach(d => userAuthenticationRepo.Delete(d));
                addedAuthentications.ForEach(d => userAuthenticationRepo.Insert(d));
                userInfo.UserAuthentications.FindAll(d => d.UserAuthenticationID != 0).ForEach(d => userAuthenticationRepo.Update(d));

                var addedUserDeviceRoles = new List<UserDeviceRole>();
                var deletedUserDeviceRoleIds = new List<int>();
                if (userInfo.UserDeviceRoles != null && userInfo.UserDeviceRoles.Any())
                {
                    var originalUserDeviceRoleIDs = originalUserInfo.UserDeviceRoles.Select(d => d.UserDeviceRoleID);
                    var userDeviceRoleIDs = userInfo.UserDeviceRoles.Select(d => d.UserDeviceRoleID);
                    deletedUserDeviceRoleIds = originalUserDeviceRoleIDs.Except(userDeviceRoleIDs).ToList();

                    addedUserDeviceRoles = userInfo.UserDeviceRoles.FindAll(d => d.UserDeviceRoleID == 0);
                }
                else
                {
                    deletedUserDeviceRoleIds = originalUserInfo.UserDeviceRoles.Select(d => d.UserDeviceRoleID).ToList();
                }

                deletedUserDeviceRoleIds.ForEach(d => userDeviceRoleRepo.Delete(d));
                addedUserDeviceRoles.ForEach(d => userDeviceRoleRepo.Insert(d));
                userInfo.UserDeviceRoles.FindAll(d => d.UserDeviceRoleID != 0).ForEach(d => userDeviceRoleRepo.Update(d));

                userPropertyRepo.Update(userInfo.UserPropertyInfo);
                userRepo.Update(userInfo);

                userEventRepo.Insert(new UserEvent()
                {
                    EventType = UserEventType.Modify,
                    UserID = userInfo.UserID,
                    CreateDate = DateTime.Now,
                    CreateUserID = GlobalSetting.WebApiUserId,
                });

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        [Authorize]
        public HttpResponseMessage Delete(int id)
        {
            return ActionWarpper.Process(id, OperationCodes.DUS, () =>
            {
                var userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
                var userPropertyRepo = RepositoryManager.GetRepository<IUserPropertyRepository>();
                var userRepo = RepositoryManager.GetRepository<IUserRepository>();
                var userDeviceRoleRepo = RepositoryManager.GetRepository<IUserDeviceRoleRepository>();
                var userEventRepo = RepositoryManager.GetRepository<IUserEventRepository>();

                var userInfo = userRepo.GetByKey(id);
                if (userInfo != null)
                {
                    userInfo.UserAuthentications.ForEach(a => userAuthenticationRepo.Delete(a.UserAuthenticationID));
                    userPropertyRepo.Delete(userInfo.UserPropertyInfo.UserPropertyID);
                    userInfo.UserDeviceRoles.ForEach(a => userDeviceRoleRepo.Delete(a.UserDeviceRoleID));
                    userRepo.Delete(id);

                    userEventRepo.Insert(new UserEvent()
                    {
                        EventType = UserEventType.Delete,
                        UserID = userInfo.UserID,
                        CreateDate = DateTime.Now,
                        CreateUserID = GlobalSetting.WebApiUserId,
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }, this);
        }

        
        public static void EncodePassword(User user)
        {
            if (user != null && user.UserAuthentications != null)
            {
                var passwords = user.UserAuthentications.FindAll(x => x.AuthenticationType == AuthenticationType.Password);
                if (passwords.Any())
                {
                    passwords.ForEach(x => x.AuthenticationData = SimpleEncryption.Encode(x.AuthenticationData));
                }
            }
        }

        public static void DecodePassword(User user)
        {
            if (user != null && user.UserAuthentications != null)
            {
                var password = user.UserAuthentications.FirstOrDefault(x => x.AuthenticationType == AuthenticationType.Password);
                if (password != null)
                {
                    password.AuthenticationData = SimpleEncryption.Decode(password.AuthenticationData);
                }
            }
        }
    }
}
