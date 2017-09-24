
/**    The feature of login and register are based on middleware.
             See code in "JwtAuthentication.middleware.cs".

            Login : [POST] -> /api/user/login
            Register : [POST] -> /api/user/register

            body : {
                        "user" : "exampleUserName",
                        "psd" : "123456"
                        }

            response: {
                              "status" : 0,
                              "message" : null,
                              "content": {
                                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtaWFvMTdnYW1lIiwianRpIjoiNzc0N2ExMTE3YjA0
                                                NDk5Y2EyOTRkYjY3MjIzNDg4NGYiLCJpYXQiOjE1MDYxNDU3OTUsImdlbmRlciI6WyJNYWxlIiwiTWFsZSJ
                                                dLCJ1dWlkIjoiNzc0N2ExMTE3YjA0NDk5Y2EyOTRkYjY3MjIzNDg4NGYiLCJ1X25hbWUiOiJtaWFvMTdnY
                                                W1lIiwicm9sZV9pZCI6IjMiLCJuYmYiOjE1MDYxNDU3OTQsImV4cCI6MTUwNjc1MDU5NCwiaXNzIjoiVG
                                                Fzc2VsX0lTUyIsImF1ZCI6IlRhc3NlbF9BVUROIn0.lWQxG16uwAOgf_rCoqEsPbk2SameTNWpu_Qn-j7mLX0",
                                "name": "miao17game",
                                "uuid": "7747a1117b04499ca294db672234884f",
                                "expires": 604800
                              }
                            }
         */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tassel.API.Utils.Extensions;
using Wallace.Core.Helpers.Controllers;
using Tassel.Model.Models;
using Tassel.Service.Utils.Extensionss;
using Tassel.Services.Contract;
using System.IdentityModel.Tokens.Jwt;
using Tassel.API.Utils.Authorization;
using Tassel.API.VM.Identity;

namespace Tassel.Service.Controllers {

    [Route("api/user")]
    public class IdentityController : Controller {

        private APIDB db;
        private IWeiboOAuthService<User> weibo;
        private IIdentityService<JwtSecurityToken, TokenProviderOptions, User> identity;

        public IdentityController(APIDB db, IIdentityService<JwtSecurityToken, TokenProviderOptions, User> isrv, IWeiboOAuthService<User> WEOBO_SRV) {
            this.db = db;
            this.weibo = WEOBO_SRV;
            this.identity = isrv;
        }

        [HttpGet]
        [UserAuthorize]
        public JsonResult GetUser() {
            var uuid = this.HttpContext.Items.FirstOrDefault(i => i.Key == (object)TokenClaimsKey.UUID).Value as string;
            if(uuid==null)
                return this.JsonFormat(false, JsonStatus.Error,"user not login.");
            var (user, succeed, error) = this.identity.GetUserDetailsByID(uuid);
            var status = succeed ? JsonStatus.Succeed : JsonStatus.UserNotFound;
            return this.JsonFormat(succeed, status, error, user);
        }

        [HttpGet("all")]
        [AdminAuthorize]
        public JsonResult GetAll() {
            return this.JsonFormat(true, content: this.identity.GetUsersListByFilter(i => true));
        }

        [HttpGet("{id}")]
        [UserAuthorize]
        public JsonResult GetUser(string id) {
            var (user, succeed, error) = this.identity.GetUserDetailsByID(id);
            var status = succeed ? JsonStatus.Succeed : JsonStatus.UserNotFound;
            return this.JsonFormat(succeed, status, error, user);
        }

        [HttpPut("{id}")]
        public void Put(string id, [FromBody]UpdateUser user) {
        }

        /// <summary>
        /// It's unavaliable to delete an user.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public JsonResult Delete(string id) {
            return this.JsonFormat(false, error: "deleting user is unavaliable.");
        }

        [HttpGet("weibo_access")]
        public async Task<JsonResult> WeiboAccess(string code, string redirect_url) {
            var (result, succeed, error) = await this.weibo.GetWeiboTokenByCodeAsync(code, redirect_url);
            if (!succeed) {
                return this.JsonFormat(false, JsonStatus.WeiboAccessFailed, error, null);
            }
            var (infos, succeed02, error02) = await this.weibo.GetWeiboUserInfosAsync(result.Uid, result.AccessToken);
            if (!succeed02) {
                return this.JsonFormat(false, JsonStatus.WeiboInfosFetchFailed, error02, null);
            }
            var (user, succ03, error03) = this.weibo.TryCreateOrGetUserByWeibo(infos);
            if (!succ03) {
                return this.JsonFormat(false, JsonStatus.WeiboUserCheckFailed, error03, null);
            }
            return this.JsonFormat(true, JsonStatus.Succeed, null, new { wuid = infos.idstr });
        }

        [HttpGet("weibo_details/{uid}")]
        public JsonResult WeiboDetails(string uid) {
            var (wuser, succeed, error) = this.weibo.SearchWeiboUserInfoByUID(uid);
            var status = succeed ? JsonStatus.Succeed : JsonStatus.WeiboDetailsNotFound;
            return this.JsonFormat(succeed, status, error, wuser);
        }

    }

}
