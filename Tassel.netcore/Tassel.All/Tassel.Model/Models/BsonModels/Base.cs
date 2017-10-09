using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Utils;

namespace Tassel.Model.Models.BsonModels {

    public enum ModelType {
        Default = 0,
        User = 1,
        Comment = 2,
        Status = 3
    }

    [JsonObject]
    public class BaseModel {

        [BsonId]
        [JsonProperty("id")]
        public string ID { get; set; } = IdentityProvider.CreateGuid();

        [BsonElement("type")]
        [JsonProperty("type")]
        public virtual ModelType Type { get; } = ModelType.Default;

        [BsonElement("c_time")]
        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    }

    [JsonObject]
    public class BaseCreator {

        [BsonElement("uuid")]
        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [BsonElement("name")]
        [JsonProperty("user_name")]
        public string UserName { get; set; }

    }

    [JsonObject]
    public class BaseComment : BaseModel {

        [BsonElement("content")]
        [JsonProperty("details")]
        public string Content { get; set; }

        public override ModelType Type { get; } = ModelType.Comment;

        [BsonElement("creator")]
        [JsonProperty("creator")]
        public BaseCreator Creator { get; set; }

    }

    [JsonObject]
    public class BasePageEntry : BaseModel {

        [BsonElement("content")]
        [JsonProperty("details")]
        public string Content { get; set; }

        [BsonElement("creator")]
        [JsonProperty("creator")]
        public BaseCreator Creator { get; set; }

        [BsonElement("likes")]
        [JsonProperty("like_users")]
        public IEnumerable<BaseCreator> Likes { get; set; } = new List<BaseCreator>();

        [BsonElement("comments")]
        [JsonProperty("comments")]
        public IEnumerable<BaseComment> Comments { get; set; } = new List<BaseComment>();

    }

}
