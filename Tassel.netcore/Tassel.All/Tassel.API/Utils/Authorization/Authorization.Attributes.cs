using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.Service.Utils.Extensionss;

namespace Tassel.API.Utils.Authorization {

    public class BearerAuthorizeAttribute : AuthorizeAttribute {

        public BearerAuthorizeAttribute() {
            base.AuthenticationSchemes = "Bearer";
        }

        public BearerAuthorizeAttribute(string policy):base(policy) {
            base.AuthenticationSchemes = "Bearer";
        }

    }

    public class UserAuthorizeAttribute : BearerAuthorizeAttribute {
        public UserAuthorizeAttribute():base(PolicyRole.User) {}
    }

    public class AdminAuthorizeAttribute : BearerAuthorizeAttribute {
        public AdminAuthorizeAttribute() : base(PolicyRole.Admin) { }
    }

    public class CoreAuthorizeAttribute : BearerAuthorizeAttribute {
        public CoreAuthorizeAttribute() : base(PolicyRole.Core) { }
    }

}
