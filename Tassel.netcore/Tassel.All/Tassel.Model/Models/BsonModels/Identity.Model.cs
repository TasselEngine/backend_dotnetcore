using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models.BsonModels {

    public enum GuidType { N, D, B, P }

    public enum Gender { Male, Female }

    public static class UserRoleConstants {
        public const string CORE = "core";
        public const string Admin = "admin";
        public const string User = "user";
    }

    [JsonObject]
    public class User {

        [BsonId]
        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [BsonElement("role")]
        [JsonProperty("role")]
        public string Role { get; set; } = UserRoleConstants.User;

        [BsonElement("u_name")]
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [BsonElement("psd")]
        [JsonIgnore]
        public string Password { get; set; }

        [BsonElement("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [BsonElement("display_name")]
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [BsonElement("f_name")]
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [BsonElement("g_name")]
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [BsonElement("gender")]
        [JsonProperty("gender")]
        public Gender? Gender { get; set; }

        [BsonElement("birth_date")]
        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [BsonElement("c_time")]
        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("u_time")]
        [JsonProperty("update_time")]
        public DateTime? UpdateTime { get; set; }

        [BsonElement("is_3rd")]
        [JsonIgnore]
        public bool IsThirdPart { get; set; } = false;

        [BsonElement("weibo_id")]
        [JsonIgnore]
        public string WeiboID { get; set; }

        [BsonElement("wechat_token")]
        [JsonIgnore]
        public string WechatToken { get; set; }

        [BsonElement("qq_token")]
        [JsonIgnore]
        public string QQToken { get; set; }

        [BsonElement("avatar")]
        [JsonIgnore]
        public string Avatar { get; set; }

    }

}
