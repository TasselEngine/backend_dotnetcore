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

namespace Tassel.API.Controllers {

    [Route("admin")]
    [Token, Admin]
    public class AdminController : Controller {

        private IIdentityService<JwtSecurityToken, TokenProviderOptions, User> identity;

        public AdminController(IIdentityService<JwtSecurityToken, TokenProviderOptions, User> isrv) {
            this.identity = isrv;
        }

        [HttpGet("users")]
        public JsonResult GetAll(string type) {
            Expression<Func<User, bool>> filter = (i => true);
            if (type != null){
                filter = i=>(i.Role == type.ToLower());
            }
            return this.JsonFormat(true, content: this.identity.GetUsersListByFilter(filter));
        }

        [HttpGet("is_admin")]
        public JsonResult CheckAdminl() {
            return this.JsonFormat(true);
        }

    }
}
