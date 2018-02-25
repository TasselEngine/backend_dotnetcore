using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Extensions;
using BWS.Utils.AspNetCore.Controllers;
using Tassel.API.Utils.Authorization;
using Tassel.Model.Models.BsonModels;
using Tassel.Service.Utils.Helpers;
using Tassel.Services.Contract;
using System.Linq.Expressions;
using Tassel.Model.Models;

namespace Tassel.API.Controllers {

    [Route("admin")]
    [Token, Admin]
    public class AdminController : Controller {

        private IIdentityService<JwtSecurityToken, TokenProviderOptions, User> identity;
        private ILogService logSrv;

        public AdminController(IIdentityService<JwtSecurityToken, TokenProviderOptions, User> isrv, ILogService logsrv) {
            this.identity = isrv;
            this.logSrv = logsrv;
        }

        [HttpGet("users")]
        public JsonResult GetAll(string type) {
            Expression<Func<User, bool>> filter = (i => true);
            if (type != null) {
                filter = i => (i.Role == type.ToLower());
            }
            return this.JsonFormat(true, content: this.identity.GetUsersListByFilter(filter));
        }

        [HttpPut("appoint")]
        public JsonResult AppointUser(string uuid) {
            var (user, succeed, error) = this.identity.GetUserDetailsByID(uuid);
            if (!succeed)
                return this.JsonFormat(false, JsonStatus.UserNotFound, error);
            if(user.Role != UserRoleConstants.User)
                return this.JsonFormat(false, JsonStatus.UserRoleNotMatch);
            (succeed, error) = this.identity.UpdateUserRole(uuid, UserRoleConstants.Admin);
            if(!succeed)
                return this.JsonFormat(false, JsonStatus.UserUpdateFailed, error);
            return this.JsonFormat(true);
        }

        [HttpPut("dismiss")]
        public JsonResult DismissUser(string uuid) {
            var (user, succeed, error) = this.identity.GetUserDetailsByID(uuid);
            if (!succeed)
                return this.JsonFormat(false, JsonStatus.UserNotFound, error);
            if (user.Role != UserRoleConstants.Admin)
                return this.JsonFormat(false, JsonStatus.UserRoleNotMatch);
            (succeed, error) = this.identity.UpdateUserRole(uuid, UserRoleConstants.User);
            if (!succeed)
                return this.JsonFormat(false, JsonStatus.UserUpdateFailed, error);
            return this.JsonFormat(true);
        }

        [HttpGet("is_admin")]
        public JsonResult CheckAdminl() {
            return this.JsonFormat(true);
        }

        [HttpGet("logs")]
        public JsonResult GetLogs(int count = 20, int skip = 0) {
            var (logs, succeed, error) = this.logSrv.GetCollections(skip: skip, take: count);
            if(!succeed)
                return this.JsonFormat(false, JsonStatus.WebLogsLoadError, error.Read());
            return this.JsonFormat(true, content: logs);
        }

    }
}
