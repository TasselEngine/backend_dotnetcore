using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tassel.Model.Models;
using Tassel.Services.Contract;
using Wallace.Core.Helpers.Providers;

namespace Tassel.Services.Service {

    public class WeiboOAuthV2Option : IWeiboOAuthV2Option {
        
        public string AppID => "185930524";
        public string AppSecret => "389e2d039b372cf2763c4842ea1c46d1";

    }

    public class WeiboOAuthService : WeiboOAuthV2Provider, IWeiboOAuthService<User> {

        private APIDB db;

        public WeiboOAuthService(APIDB db, IWeiboOAuthV2Option options) : base(options) {
            this.options = options;
            this.db = db;
        }

        public (User, bool, string) TryCreateOrGetUserByWeibo(WeiboUser wuser) {
            var usrr = db.Users.Where(i => i.WeiboID == wuser.idstr).FirstOrDefault();
            if (usrr != null)
                return (usrr, true, "user is exist already");
            usrr = IdentityProvider.CreateUserByWeibo(wuser);
            var wusr = WeiboUserProvider.CreateUser(
                wuser.idstr, wuser.screen_name, wuser.description, wuser.domain, wuser.avatar_large);
            db.Add(usrr);
            db.Add(wusr);
            if (db.SaveChanges() <= 0)
                return (null, false, "save user informations failed");
            return (usrr, true, null);
        }

        public (User, bool, string) TryGetUserByWeibo(string uid) {
            var usr = db.Users.Where(i => i.WeiboID == uid).FirstOrDefault();
            if (usr == null)
                return (null, false, "user not found");
            return (usr, true, null);
        }

        public (WeiboDBUser, bool, string) SearchWeiboUserInfoByUID(string uid) {
            var wuser = db.WeiboUsers.Where(i => i.UID == uid).FirstOrDefault();
            if (wuser == null)
                return (null, false, "weibo user info is not found");
            return (wuser, true, null);
        }
    }
}
