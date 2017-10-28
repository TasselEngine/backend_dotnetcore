using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Extensions;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Utils.Constants;

namespace Tassel.API.Utils.Authorization {

    public class TokenAttribute : AuthorizeAttribute {

        public TokenAttribute() {
            base.AuthenticationSchemes = "Bearer";
        }

        public TokenAttribute(string policy):base(policy) {
            base.AuthenticationSchemes = "Bearer";
        }

    }

    public class RoleFilterAttribute : ActionFilterAttribute {

        private string[] roles;
        private readonly bool work;

        public RoleFilterAttribute(bool work = true, params string[] role) {
            this.work = work;
            this.roles = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            if (!this.work) { return; }
            context.HttpContext.GetStringEntry(TokenClaimsKey.RoleID, out var role);
            var enabled = this.roles.Contains(role);
            if(!enabled)
                context.Result = (context.Controller as Controller).JsonFormat(false, JsonStatus.UserAccessDenied);
        }

    }

    public class UserAttribute : RoleFilterAttribute {
        public UserAttribute(bool work = true) : base(work, UserRoleConstants.User, UserRoleConstants.Admin, UserRoleConstants.CORE) { }
    }

    public class AdminAttribute : RoleFilterAttribute {
        public AdminAttribute(bool work = true) : base(work, UserRoleConstants.CORE, UserRoleConstants.Admin) { }
    }

    public class CoreAttribute : RoleFilterAttribute {
        public CoreAttribute(bool work = true) : base(work, UserRoleConstants.User) { }
    }

}
