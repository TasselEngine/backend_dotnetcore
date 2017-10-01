﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Tassel.Model.Models {

    [DataContract]
    public class JsonBase : Dictionary<string, object> {

        public JsonBase() {
            this.Status = JsonStatus.Error;
            this.Message = null;
            this.Content = null;
        }

        public JsonBase(string message) : base() => this.Message = message;

        [DataMember(Name = "status")]
        public JsonStatus Status {
            get => (JsonStatus)this["Status"];
            set => this["Status"] = value;
        }

        [DataMember(Name = "msg")]
        public string Message {
            get => this["Message"] as string;
            set => this["Message"] = value;
        }

        [DataMember(Name = "content")]
        public object Content {
            get => this["Content"];
            set => this["Content"] = value;
        }

    }

    public enum JsonStatus {
        Succeed = 0,
        Error = 10000,
        LoginFailed = 20001,
        RegisterFailed = 20002,
        UserNotFound = 20003,
        UserExist = 20004,
        UserNotLogin = 20005,
        BearerCheckFailed = 20006,
        WeiboAccessFailed = 21001,
        WeiboInfosFetchFailed = 21002,
        WeiboUserCheckFailed = 21003,
        WeiboDetailsNotFound = 21004,
        WeiboRevokeFailed = 21005,
        WeiboRevokeException = 21006,
    }

    public static class JsonErrorMaps {

        private static Dictionary<JsonStatus, string> maps = new Dictionary<JsonStatus, string> {
            {JsonStatus.Succeed, "success" },
            { JsonStatus.LoginFailed, "login failed." },
            { JsonStatus.RegisterFailed, "register failed." },
            { JsonStatus.UserNotFound, "user not found." },
            { JsonStatus.UserExist, "user is exist." },
            { JsonStatus.UserNotLogin, "user is not login." },
            { JsonStatus.WeiboAccessFailed, "try to get weibo access_token failed." },
            { JsonStatus.WeiboInfosFetchFailed, "try to fetch weibo user info failed." },
            { JsonStatus.WeiboUserCheckFailed, "try to checkin weibo user failed." },
            { JsonStatus.BearerCheckFailed, "bearer token check failed."},
            { JsonStatus.WeiboRevokeFailed, "revoke oauth 2.0 failed." },
            { JsonStatus.WeiboRevokeException, "revoke from oauth 2.0 doesn't work well." },
            { JsonStatus.Error, "unknown error." },
        };

        public static string TryGet(JsonStatus type) {
            return maps.GetValueOrDefault(type) ?? "";
        }

    }

}
