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

namespace Tassel.API.Controllers {

    [Route("api/admin")]
    [Token, Admin]
    public class AdminController : Controller {

        private IIdentityService<JwtSecurityToken, TokenProviderOptions, User> identity;

        public AdminController(IIdentityService<JwtSecurityToken, TokenProviderOptions, User> isrv) {
            this.identity = isrv;
        }

        [HttpGet("users")]
        public JsonResult GetAll() {
            return this.JsonFormat(true, content: this.identity.GetUsersListByFilter(i => true));
        }

        [HttpGet("is_admin")]
        public JsonResult CheckAdminl() {
            return this.JsonFormat(true);
        }

    }
}
