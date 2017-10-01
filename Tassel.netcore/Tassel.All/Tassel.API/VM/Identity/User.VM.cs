﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Tassel.Model.Models;

namespace Tassel.API.VM.Identity {

    public enum UserVMType { Base, Weibo, Wechat, QQ }

    public delegate (WeiboDBUser, bool, string) WeiboUserDetailsHandler(string weibo_id);

    [JsonObject]
    public class UserVM {

        public UserVM(User user) {
            this.user = user;
            if (user == null)
                throw new ArgumentNullException("user shouldn't be empty");
            if (!user.IsThirdPart) { return; }
            this.type =
                !string.IsNullOrEmpty(user.WeiboID) ? UserVMType.Weibo :
                !string.IsNullOrEmpty(user.WechatToken) ? UserVMType.Wechat :
                !string.IsNullOrEmpty(user.QQToken) ? UserVMType.QQ :
                UserVMType.Base;
        }

        private User user;

        private UserVMType type = UserVMType.Base;
        [JsonIgnore]
        public UserVMType UserType { get => this.type; }

        private (bool, string) check = (true, null);
        [JsonIgnore]
        public (bool ,string) Check { get => this.check; }

        [JsonProperty("user")]
        public DynamicUser User { get; private set; }

        public UserVM Create(WeiboUserDetailsHandler handler) {
            this.Create();
            var (wuser, succeed, error) = handler(user.WeiboID);
            this.check = (succeed, error);
            this.User.ScreenName = wuser.ScreenName;
            this.User.AccessToken = wuser.AccessToken;
            this.User.AvatarUrl = wuser.AvatarUrl;
            this.User.Domain = wuser.Domain;
            this.User.Description = wuser.Description;
            return this;
        }

        public UserVM Create() {
            if (user == null)
                throw new InvalidOperationException("user shouldn't be empty");
            this.User = new DynamicUser {
                UUID = this.user.UUID,
                RoleID = this.user.RoleID,
                UserName = this.user.UserName,
                BirthDate = this.user.BirthDate,
                CreateTime = this.user.CreateTime,
                DisplayName = this.user.DisplayName,
                Email = this.user.Email,
                FamilyName = this.user.FamilyName,
                Gender = this.user.Gender,
                GivenName = this.user.GivenName,
                UpdateTime = this.user.UpdateTime,
                Avatar = this.user.Avatar,
                IsThirdPart = this.user.IsThirdPart,
                UserType = this.type,
            };
            return this;
        }

    }

    [JsonObject]
    public class DynamicUser {

        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [JsonProperty("role_id")]
        public int RoleID { get; set; } = 3;

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("gender")]
        public Gender? Gender { get; set; }

        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("update_time")]
        public DateTime? UpdateTime { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("is_third_part")]
        public bool IsThirdPart { get; set; } = false;

        [JsonProperty("user_type")]
        public UserVMType UserType { get; set; } = UserVMType.Base;

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

    }
}
