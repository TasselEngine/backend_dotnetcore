using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Tassel.Service.Utils.Helpers;
using Tassel.Services.Contract;
using System.Linq.Expressions;
using BWS.Utils.NetCore.Format;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Tassel.Model.Utils;
using Tassel.Services.Utils.Constants;
using Tassel.Model.Models.BsonModels;
using MongoDB.Driver;
using Tassel.Model.Models;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Service {
    public class IdentityService : IIdentityService<JwtSecurityToken, TokenProviderOptions, User> {

        protected IMongoDatabase mdb;
        protected IMongoCollection<User> users;
        private IHostingEnvironment env;
        private IWeiboOAuthServiceProvider<User> _WEOBO_SRV;

        public IdentityService(MongoDBContext db, IHostingEnvironment env, IWeiboOAuthServiceProvider<User> WEOBO_SRV) {
            this.env = env;
            this.mdb = db.DB;
            this.users = mdb.GetCollection<User>(ModelCollectionName.User);
            this._WEOBO_SRV = WEOBO_SRV;
        }

        public IWeiboOAuthServiceProvider<User> WeiboService => this._WEOBO_SRV;

        public JwtSecurityToken GenerateToken(User user, TokenProviderOptions options) {
            return new JwtSecurityToken(
                 issuer: options.Issuer,
                 audience: options.Audience,
                 claims: new Claim[]{
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, user.UUID),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnix().ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.Gender, user.Gender.ToString()),
                    new Claim(TokenClaimsKey.Avatar,user.Avatar??""),
                    new Claim(TokenClaimsKey.UUID,user.UUID),
                    new Claim(TokenClaimsKey.UserName,user.UserName??""),
                    new Claim(TokenClaimsKey.RoleID,user.Role??"".ToString()),
                 },
                 notBefore: DateTime.UtcNow,
                 expires: DateTime.UtcNow.Add(options.Expiration),
                 signingCredentials: options.SigningCredentials);
        }

        public (User, bool, string) GetUserDetailsByID(string uuid) {
            var usr = this.users.Find(i => i.UUID == uuid).FirstOrDefault();
            if (usr == null)
                return (null, false, Errors.UserNotFound);
            return (usr, true, null);
        }

        public (User, bool, string) GetUserDetailsByUserName(string uname) {
            var usr = this.users.Find(i => i.UserName == uname).FirstOrDefault();
            if (usr == null)
                return (null, false, Errors.UserNotFound);
            return (usr, true, null);
        }

        public (string role, bool succeed, string error) GetUserRole(string uuid) {
            var (user, succeed, error) = this.GetUserDetailsByID(uuid);
            if (!succeed)
                return (null, false, error);
            return (user.Role, true, error);
        }

        public IEnumerable<dynamic> GetUsersListByFilter(Expression<Func<User, bool>> whereLambada) {
            return this.users.AsQueryable().Where(whereLambada)?.Select(i => new {
                i.UUID,
                i.UserName,
                i.DisplayName,
                i.Gender,
                i.Avatar,
                i.Role,
                i.IsThirdPart,
                i.WeiboID,
                i.WechatToken,
                i.QQToken,
                i.CreateTime
            });
        }

        public JwtSecurityToken TokenDecrypt(string cookie) {
            var (tk, _) = new TokenDecoder(env).Decrypt(cookie);
            return tk;
        }

        public (User, bool, string) TryLogin(string user, string psd) {
            var usr = this.users.Find(i => i.UserName == user).FirstOrDefault();
            if (usr == null)
                return (null, false, Errors.UserNotFound);
            if (usr.Password != IdentityProvider.CreateMD5(psd))
                return (null, false, Errors.PasswordNotCorrect);
            return (usr, true, null);
        }

        public (User, bool, string) TryRegister(string user, string psd, Gender gender = Gender.Male, string avatar = null) {
            var usrr = this.users.Find(i => i.UserName == user).FirstOrDefault();
            if (usrr != null)
                return (null, false, Errors.UserExist);
            usrr = IdentityProvider.CreateUser(user, psd, gender, avatar);
            try {
                this.users.InsertOne(usrr);
                return (usrr, true, null);
            } catch (Exception e) {
                return (null, false, Errors.SaveUserInfosFailed + $" : ${e.Message}");
            }
        }

        public (bool succeed, string error) TryUpdateNative(User user) {
            try {
                this.users.InsertOne(user);
                return (true, null);
            } catch (Exception e) {
                return (false, Errors.UpdateUserFailed + $" : ${e.Message}");
            }
        }

        public (bool succeed, string error) UpdateUserRole(string user_id, string newRole) {
            try {
                var result = this.users.FindOneAndUpdate(i => i.UUID == user_id, this.CreateUpdate().Set(i => i.Role, newRole));
                if (result == null)
                    return (false, $"${Errors.UserUpdateFailed} : user to be updated is not exist.");
                return (true, null);
            } catch (Exception e) {
                return (false, $"${Errors.UserUpdateFailed} : ${e.Message}");
            }
        }

        protected virtual UpdateDefinition<User> CreateUpdate(User entry = null) {
            var upts = Builders<User>.Update.Set(i => i.UpdateTime, (entry?.UpdateTime)?? DateTime.UtcNow);
            return upts;
        }

    }
}
