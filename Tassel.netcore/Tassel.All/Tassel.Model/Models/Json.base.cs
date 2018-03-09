using BWS.Utils.AspNetCore.JsonResult;
using System;
using System.Collections.Generic;
using System.Linq;
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
        BodyFormIsNull = 10002,
        QueryParamsNull = 10003,

        BearerCheckFailed = 20001,
        LoginFailed = 20002,
        RegisterFailed = 20003,
        UserNotFound = 20004,
        UserExist = 20005,
        UserNotLogin = 20006,
        UserNotMatched = 20007,
        UserUpdateFailed = 20008,
        ThirdPartUserNotExist = 20009,
        UserAccessDenied = 20010,
        CheckAdminFailed = 20011,
        UserRoleNotMatch = 20012,

        WeiboAccessFailed = 21001,
        WeiboInfosFetchFailed = 21002,
        WeiboUserCheckFailed = 21003,
        WeiboDetailsNotFound = 21004,
        WeiboRevokeFailed = 21005,
        WeiboRevokeException = 21006,

        WebLogsLoadError = 25001,

        MessageCreateFailed = 26001,
        GetMessagesFailed = 26002,
        ReadMessageFailed = 26003,

        InsertEntryFailed = 30001,
        DeleteEntryFailed = 30002,
        EntryNotFound = 30003,

        CreateImageFailed = 31001,

        StatusCollectionLoadFailed = 40001,
        StatusInsertFailed = 40002,
        StatusNotFound = 40003,

        CommentAddFailed = 50001,
        CommentRemoveFailed = 50002,
        LikesAddFailed = 51101,
        LikesRemoveFailed = 51102,
    }

    public static class JsonErrorMaps {

        private static Dictionary<JsonStatus, string> maps = new Dictionary<JsonStatus, string> {
            [JsonStatus.Succeed] = "success",
            [JsonStatus.Error] = "unknown error.",
            [JsonStatus.LoginFailed] = "login failed.",
            [JsonStatus.DeleteNotAllowed] = Errors.DeleteNotAllowed,
            [JsonStatus.BodyFormIsNull] = "the form of request body shouldn't be empry, or your input is invalid.",

            [JsonStatus.QueryParamsNull] = "the query parameters shouldn't be empry.",
            [JsonStatus.RegisterFailed] = "register failed.",
            [JsonStatus.UserNotFound] = Errors.UserNotFound,
            [JsonStatus.UserExist] = Errors.UserExist,
            [JsonStatus.UserNotLogin] = "user is not login.",
            [JsonStatus.UserNotMatched] = "the uuid and logined user are not matched.",
            [JsonStatus.UserUpdateFailed] = Errors.UpdateUserFailed,
            [JsonStatus.ThirdPartUserNotExist] = "3rd-part user details not found.",
            [JsonStatus.UserAccessDenied] = "the role of user logined is denied in this request.",
            [JsonStatus.CheckAdminFailed] = "check user admin role failed.",
            [JsonStatus.UserRoleNotMatch]="the role of user is not matched, action failed.",

            [JsonStatus.WeiboAccessFailed] = "try to get weibo access_token failed.",
            [JsonStatus.WeiboInfosFetchFailed] = "try to fetch weibo user info failed.",
            [JsonStatus.WeiboUserCheckFailed] = "try to checkin weibo user failed.",
            [JsonStatus.BearerCheckFailed] = "bearer token check failed.",
            [JsonStatus.WeiboRevokeFailed] = "revoke oauth 2.0 failed.",
            [JsonStatus.WeiboRevokeException] = "revoke from oauth 2.0 doesn't work well.",

            [JsonStatus.WebLogsLoadError]="load web running logs failed.",

            [JsonStatus.MessageCreateFailed] = "try to add new message failed.",
            [JsonStatus.GetMessagesFailed] = "try to load message list failed.",
            [JsonStatus.ReadMessageFailed] = "try to read message(s) failed.",

            [JsonStatus.InsertEntryFailed] = Errors.InsertOneFailed,
            [JsonStatus.DeleteEntryFailed] = Errors.DeleteEntryFailed,
            [JsonStatus.EntryNotFound] = "entry not found.",

            [JsonStatus.CreateImageFailed] = "create static resources of images failed",

            [JsonStatus.StatusCollectionLoadFailed] = "read status collection failed",
            [JsonStatus.StatusInsertFailed] = "add status failed.",
            [JsonStatus.StatusNotFound] = "status with the id is not found.",

            [JsonStatus.CommentAddFailed] = "add comment failed.",
            [JsonStatus.CommentRemoveFailed] = "remove comment failed.",
            [JsonStatus.LikesAddFailed] = "add a like relation failed.",
            [JsonStatus.LikesRemoveFailed] = "remove a like relation failed.",
        };

        public static string TryGet(JsonStatus type) {
            return maps.GetValueOrDefault(type) ?? "";
        }

    }

}
