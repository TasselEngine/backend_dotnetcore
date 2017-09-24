﻿using System;
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

        public (User, bool, string) TryCreateOrUpdateUserByWeibo(WeiboUser wuser) {
            var usrr = db.Users.Where(i => i.WeiboID == wuser.idstr).FirstOrDefault();
            if (usrr != null) {
                var wusr = db.WeiboUsers.Where(i => i.UID == wuser.idstr).FirstOrDefault();
                if (wusr == null) {
                    db.Add(WeiboUserProvider.CreateUser(wuser));
                } else {
                    var id = wusr.ID;
                    wusr = WeiboUserProvider.CreateUser(wuser);
                    wusr.ID = id;
                    db.Update(wusr);
                }
            } else {
                db.Add(IdentityProvider.CreateUserByWeibo(wuser));
                db.Add(WeiboUserProvider.CreateUser(wuser));
            }
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
