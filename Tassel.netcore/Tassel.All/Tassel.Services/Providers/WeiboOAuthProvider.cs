using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract.Providers;
using WeiboOAuth2.Provider;

namespace Tassel.Services.Providers {

    public class WeiboOAuthV2Option : IWeiboOAuthV2Option {

        public string AppID => "185930524";
        public string AppSecret => "389e2d039b372cf2763c4842ea1c46d1";

    }

    public class WeiboOAuthProvider : WeiboOAuthV2Provider, IWeiboOAuthServiceProvider<User> {

        protected IMongoDatabase mdb;
        protected IMongoCollection<User> users;
        protected IMongoCollection<WeiboDBUser> WEIBOS;

        public WeiboOAuthProvider(MongoDBContext db, IWeiboOAuthV2Option options) : base(options) {
            this.mdb = db.DB;
            this.users = mdb.GetCollection<User>(ModelCollectionName.User);
            this.WEIBOS = mdb.GetCollection<WeiboDBUser>(ModelCollectionName.Weibo);
        }

        public (User, bool, string) TryCreateOrUpdateUserByWeibo(WeiboUser wuser, string access_token) {
            var usrr = this.users.Find(i => i.WeiboID == wuser.idstr).FirstOrDefault();
            try {
                if (usrr != null) {
                    var wusr = this.WEIBOS.Find(i => i.UID == wuser.idstr).FirstOrDefault();
                    if (wusr == null)
                        this.WEIBOS.InsertOne(WeiboUserProvider.CreateUser(wuser, access_token));
                    else
                        this.WEIBOS.UpdateOne(i => i.UID == wuser.idstr, this.CreateUpdate(wusr.Update(wuser)));
                } else {
                    this.users.InsertOne(IdentityProvider.CreateUserByWeibo(wuser));
                    this.WEIBOS.InsertOne(WeiboUserProvider.CreateUser(wuser, access_token));
                }
                return (usrr, true, null);
            } catch(Exception e) {
                return (null, false, Errors.SaveUserInfosFailed + $" : ${e.Message}");
            }
        }

        public (User, bool, string) TryGetUserByWeibo(string uid) {
            var usr = this.users.Find(i => i.WeiboID == uid).FirstOrDefault();
            if (usr == null)
                return (null, false, Errors.UserNotFound);
            return (usr, true, null);
        }

        public (WeiboDBUser, bool, string) SearchWeiboUserInfoByUID(string uid) {
            var wuser = this.WEIBOS.Find(i => i.UID == uid).FirstOrDefault();
            if (wuser == null)
                return (null, false, Errors.WeiboUserNotFound);
            return (wuser, true, null);
        }

        protected virtual UpdateDefinition<WeiboDBUser> CreateUpdate(WeiboDBUser entry) {
            var upts = Builders<WeiboDBUser>.Update
                .Set(i => i.UpdateTime, entry.UpdateTime)
                .Set(i => i.ScreenName, entry.ScreenName)
                .Set(i => i.Domain, entry.Domain)
                .Set(i => i.Description, entry.Description)
                .Set(i => i.CoverMobile, entry.CoverMobile)
                .Set(i => i.Cover, entry.Cover)
                .Set(i => i.AvatarUrl, entry.AvatarUrl)
                .Set(i => i.AccessToken, entry.AccessToken);
            return upts;
        }

    }
}
