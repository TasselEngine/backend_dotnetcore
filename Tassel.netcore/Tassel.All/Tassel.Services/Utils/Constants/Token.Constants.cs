using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Services.Utils.Constants {
    public struct PolicyRole {
        public const string Core = "CORE";
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public struct TokenClaimsKey {
        public const string RoleID = "role_id";
        public const string Gender = "gender";
        public const string UserName = "u_name";
        public const string Password = "psd";
        public const string UUID = "uuid";
        public const string Avatar = "avatar";
    }

    public struct TokenProviderEntry {
        public const string Issuer = "Tassel_ISS";
        public const string Audience = "Tassel_AUDN";
        public const string TokenName = "smhs_token";
        public const string CookieScheme = "Tassel_Cookie";
        public const string RegisterPath = "user/register";
        public const string LoginPath = "user/login";
        public const string WeiboCheckPath = "user/weibo_checkin";
        public const string AccessDenied = "/403";
    }

}
