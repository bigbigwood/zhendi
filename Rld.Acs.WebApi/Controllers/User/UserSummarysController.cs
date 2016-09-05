using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
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
    public class UserSummarysController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HttpResponseMessage Get()
        {
            var conditions = ControllerContext.Request.GetQueryNameValueHashtable();
            return ActionWarpper.Process(conditions, OperationCodes.QUSSMY, () =>
            {
                IEnumerable<User> users = new List<User>();
                if (conditions.ContainsKey("DepartmentID"))
                {
                    var repo = RepositoryManager.GetRepository<IUserRepository>();
                    users = repo.GetDepartmentSummaryUsers(conditions["DepartmentID"].ToInt32());
                }

                return Request.CreateResponse(HttpStatusCode.OK, users.ToList());
               
            }, this);
        }
    }
}
