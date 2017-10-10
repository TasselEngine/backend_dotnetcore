﻿using BWS.Utils.AspNetCore.JsonResult;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Tassel.Model.Utils;

namespace Tassel.Model.Models {

    public class JsonBase : JsonBase<JsonStatus> {

        public JsonBase() {
            this.Status = JsonStatus.Error;
        }

        public JsonBase(string message) : base(message) { }

    }

    public enum JsonStatus {
        Succeed = 0,

        Error = 10000,
        DeleteNotAllowed = 10001,

        BearerCheckFailed = 20001,
        LoginFailed = 20002,
        RegisterFailed = 20003,
        UserNotFound = 20004,
        UserExist = 20005,
        UserNotLogin = 20006,
        UserNotMatched = 20007,
        UserUpdateFailed = 20008,
        ThirdPartUserNotExist = 20009,

        WeiboAccessFailed = 21001,
        WeiboInfosFetchFailed = 21002,
        WeiboUserCheckFailed = 21003,
        WeiboDetailsNotFound = 21004,
        WeiboRevokeFailed = 21005,
        WeiboRevokeException = 21006,

        InsertEntryFailed = 30001,

        StatusCollectionLoadFailed = 40001,
        StatusInsertFailed = 40002,
    }

    public static class JsonErrorMaps {

        private static Dictionary<JsonStatus, string> maps = new Dictionary<JsonStatus, string> {
            [JsonStatus.Succeed] = "success",
            [JsonStatus.LoginFailed] = "login failed.",
            [JsonStatus.DeleteNotAllowed] = Errors.DeleteNotAllowed,
            [JsonStatus.RegisterFailed] = "register failed.",
            [JsonStatus.UserNotFound] = Errors.UserNotFound,
            [JsonStatus.UserExist] = Errors.UserExist,
            [JsonStatus.UserNotLogin] = "user is not login.",
            [JsonStatus.UserNotMatched] = "the uuid and logined user are not matched.",
            [JsonStatus.UserUpdateFailed] = Errors.UpdateUserFailed,
            [JsonStatus.ThirdPartUserNotExist] = "3rd-part user details not found.",
            [JsonStatus.WeiboAccessFailed] = "try to get weibo access_token failed.",
            [JsonStatus.WeiboInfosFetchFailed] = "try to fetch weibo user info failed.",
            [JsonStatus.WeiboUserCheckFailed] = "try to checkin weibo user failed.",
            [JsonStatus.BearerCheckFailed] = "bearer token check failed.",
            [JsonStatus.WeiboRevokeFailed] = "revoke oauth 2.0 failed.",
            [JsonStatus.WeiboRevokeException] = "revoke from oauth 2.0 doesn't work well.",
            [JsonStatus.Error] = "unknown error.",
            [JsonStatus.InsertEntryFailed] = Errors.InsertOneFailed,
            [JsonStatus.StatusCollectionLoadFailed] = "read status collection failed",
            [JsonStatus.StatusInsertFailed] = "add status failed."
        };

        public static string TryGet(JsonStatus type) {
            return maps.GetValueOrDefault(type) ?? "";
        }

    }

}
