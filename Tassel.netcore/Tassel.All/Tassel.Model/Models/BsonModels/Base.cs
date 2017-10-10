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

    public static class ModelCollectionName {
        public const string Status = "status";
    }

    [JsonObject]
    public class BaseModel {

        [BsonId]
        [JsonProperty("id")]
        public string ID { get; set; } = IdentityProvider.CreateGuid(GuidType.N);

        [BsonElement("type")]
        [JsonProperty("type")]
        public virtual ModelType Type { get; } = ModelType.Default;

        [BsonElement("c_time")]
        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("u_time")]
        [JsonProperty("update_time")]
        public DateTime? UpdateTime { get; set; }

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
    public class BaseCreateModel: BaseModel {

        [BsonElement("creator")]
        [JsonProperty("creator")]
        public BaseCreator Creator { get; set; }

    }

    [JsonObject]
    public class BaseLikesModel : BaseCreateModel {

        [BsonElement("likes")]
        [JsonProperty("like_users")]
        public IEnumerable<BaseCreator> Likes { get; set; } = new List<BaseCreator>();

    }

    [JsonObject]
    public class Comment : BaseCreateModel {

        [BsonElement("content")]
        [JsonProperty("details")]
        public string CommentContent { get; set; }

        public override ModelType Type { get; } = ModelType.Comment;

    }

    [JsonObject]
    public class BasePageEntry : BaseLikesModel {

        [BsonElement("content")]
        [JsonProperty("details")]
        public string Content { get; set; }

    }

}
