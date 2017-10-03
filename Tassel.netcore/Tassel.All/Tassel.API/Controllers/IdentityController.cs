
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
using BWS.Utils.AspNetCore.Controllers;
using Tassel.Model.Models;
using Tassel.Services.Contract;
using System.IdentityModel.Tokens.Jwt;
using Tassel.API.Utils.Authorization;
using Tassel.API.VM.Identity;
using Tassel.Model.Utils;
using Tassel.Service.Utils.Helpers;
using Tassel.Services.Utils.Constants;

namespace Tassel.Service.Controllers {

    [Route("api/user")]
    public class IdentityController : Controller {

        private IIdentityService<JwtSecurityToken, TokenProviderOptions, User> identity;

        public IdentityController(IIdentityService<JwtSecurityToken, TokenProviderOptions, User> isrv) {
            this.identity = isrv;
        }

        [HttpGet]
        [UserAuthorize]
        public JsonResult GetUser() {
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            if(uuid==null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            return this.GetUser(uuid);
        }

        [HttpGet("all")]
        [AdminAuthorize]
        public JsonResult GetAll() {
            return this.JsonFormat(true, content: this.identity.GetUsersListByFilter(i => true));
        }

        [HttpGet("{uuid}")]
        [UserAuthorize]
        public JsonResult GetUser(string uuid) {
            var (user, succeed, error) = this.identity.GetUserDetailsByID(uuid);
            if (user == null)
                return this.JsonFormat(false, JsonStatus.UserNotFound);
            var content = new UserVM(user);
            var status = JsonStatus.Succeed;
            if (user.IsThirdPart) {
                if (content.UserType == UserVMType.Weibo) { 
                    // Load weibo user details. To extend this method if more 3rd-part added.
                    (succeed, error) = content.Create(this.identity.WeiboService.SearchWeiboUserInfoByUID).Check;
                    status = succeed ? JsonStatus.Succeed : JsonStatus.WeiboDetailsNotFound;
                } else { 
                    // No 3rd-part user infos found, action failed.
                    succeed = false;
                    status = JsonStatus.ThirdPartUserNotExist;
                    error = JsonErrorMaps.TryGet(status);
                }
            }
            return this.JsonFormat(succeed, status, error, content);
        }

        [HttpPut("{uuid}")]
        [UserAuthorize]
        public void Put(string uuid, [FromBody]UpdateUser uuser) {
        }

        [HttpPut("user_native/{uuid}")]
        [UserAuthorize]
        public JsonResult UserNative(string uuid, [FromBody]NativeUser nuser) {
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var p_uuid);
            if (p_uuid == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            if(uuid != p_uuid)
                return this.JsonFormat(false, JsonStatus.UserNotMatched);
            var (user, succeed, error) = this.identity.GetUserDetailsByID(uuid);
            if (user == null)
                return this.JsonFormat(false, JsonStatus.UserNotFound, error);
            user.UserName = nuser.UserName;
            user.Password = IdentityProvider.CreateMD5(nuser.Password);
            user.DisplayName = nuser.DisplayName;
            (succeed, error) = this.identity.TryUpdate(user);
            if(!succeed)
                return this.JsonFormat(false, JsonStatus.UserUpdateFailed, error);
            return this.JsonFormat(true);
        }

        [HttpDelete("{uuid}")]
        public JsonResult Delete(string uuid) {
            return this.JsonFormat(false, JsonStatus.DeleteNotAllowed, Errors.DeleteNotAllowed);
        }

        [HttpGet("weibo_access")]
        public async Task<JsonResult> WeiboAccess(string code, string redirect_url) {
            var (result, succeed, error) = await this.identity.WeiboService.GetWeiboTokenByCodeAsync(code, redirect_url);
            if (!succeed) 
                return this.JsonFormat(false, JsonStatus.WeiboAccessFailed, error);
            var (infos, succ02, error02) = await this.identity.WeiboService.GetWeiboUserInfosAsync(result.Uid, result.AccessToken);
            if (!succ02) 
                return this.JsonFormat(false, JsonStatus.WeiboInfosFetchFailed, error02);
            var (user, succ03, error03) = this.identity.WeiboService.TryCreateOrUpdateUserByWeibo(infos, result.AccessToken);
            if (!succ03) 
                return this.JsonFormat(false, JsonStatus.WeiboUserCheckFailed, error03);
            return this.JsonFormat(true, JsonStatus.Succeed, content : new { wuid = infos.idstr });
        }

        [HttpGet("weibo_details/{wuid}")]
        public JsonResult WeiboDetails(string wuid) {
            var (wuser, succeed, error) = this.identity.WeiboService.SearchWeiboUserInfoByUID(wuid);
            if(!succeed)
                return this.JsonFormat(false, JsonStatus.WeiboDetailsNotFound, error);
            return this.JsonFormat(true, content : wuser);
        }

        [HttpGet("weibo_revoke/{access_token}")]
        public async Task<JsonResult > WeiboOAuth2Revoke(string access_token) {
            var (result, succeed, error) = await this.identity.WeiboService.RevokeOAuth2Access(access_token);
            if(!succeed)
                return this.JsonFormat(false, JsonStatus.WeiboRevokeFailed, error, result);
            if (result != null && !result.Return)
                return this.JsonFormat(true, JsonStatus.WeiboRevokeException, error, result);
            return this.JsonFormat(true, content : result);
        }

    }

}
