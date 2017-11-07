using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models.BsonModels {

    public enum LogLevel {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3,
        Fatal = 4
    }

    public enum LogAction {
        Insert = 0,
        Update = 1,
        Delete = 2,
        Publish = 4,
        Unpublish = 5,
        AddBlackList = 6,
        TagDelete = 7,
        Appoint = 8,
        Dismiss = 9,
        Register = 10,
        UserNative = 11,
        Other = 99,
    }

    public enum LogRole {
        User = 2,
        Admin = 1,
        Core = 0,
        Error = -1
    }

    [JsonObject]
    public class LogEntry : BaseCreateModel {

        public override ModelType Type { get; } = ModelType.Log;

        [BsonElement("level")]
        [JsonProperty("level")]
        public LogLevel Level { get; set; } = LogLevel.Info;

        [BsonElement("role")]
        [JsonProperty("role")]
        public LogRole Role { get; set; } = LogRole.User;

        [BsonElement("action")]
        [JsonProperty("action")]
        public LogAction Action { get; set; } = LogAction.Other;

        [BsonElement("ttype")]
        [JsonProperty("target_type")]
        public ModelType TargetType { get; set; } = ModelType.Default;

        [BsonElement("tid")]
        [JsonProperty("target_id")]
        public string TargetID { get; set; }

        [BsonElement("tkey")]
        [JsonProperty("target_key")]
        public string TargetKey { get; set; }

        [BsonElement("desc")]
        [JsonProperty("desc")]
        public string Description { get; set; }

        public bool ShouldSerializeUpdateTime() => false;

    }

    public struct Agent {

        public readonly string UID;
        public readonly string UserName;
        public readonly string Avatar;

        public static Agent Unknown { get; } = new Agent(null, "unknown");

        public Agent(string uid, string user, string avatar = null) {
            this.UID = uid;
            this.UserName = user;
            this.Avatar = avatar;
        }
    }

    public struct Target {

        public readonly string ID;
        public readonly string Key;
        public readonly string Description;

        public static Target Unknown { get; } = new Target(null, "unknown");

        public Target(string id, string key, string desc = null) {
            this.ID = id;
            this.Key = key;
            this.Description = desc;
        }
    }

}
