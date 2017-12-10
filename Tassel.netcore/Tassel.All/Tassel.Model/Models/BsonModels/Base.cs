using BWS.Utils.NetCore.Format;
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
        WeiboUser = 2,
        Comment = 11,
        LikeEntry = 12,
        Status = 13,
        Post = 14,
        Log = 31,
    }

    public enum EntryState {
        Published = 0,
        Unpublished = 1,
        Blacklist = 2,
        Deleted = 3
    }

    public static class ModelCollectionName {
        public const string User = "users";
        public const string Weibo = "weibos";
        public const string Comment = "comments";
        public const string Likes = "likes";
        public const string Status = "status";
        public const string Log = "logs";
    }

    [JsonObject]
    public class BaseModel {

        [BsonId]
        [JsonProperty("id")]
        public virtual string ID { get; set; } = IdentityProvider.CreateGuid(GuidType.N);

        [BsonElement("type")]
        [JsonProperty("type")]
        public virtual ModelType Type { get; } = ModelType.Default;

        [BsonElement("c_time")]
        [JsonProperty("create_time")]
        public virtual long CreateTime { get; set; } = DateTime.UtcNow.ToUnix();

        [BsonElement("u_time")]
        [JsonProperty("update_time")]
        public virtual long? UpdateTime { get; set; }

    }

    [JsonObject]
    public class BaseCreator {

        [BsonElement("uuid")]
        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [BsonElement("name")]
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [BsonElement("avatar")]
        [JsonProperty("avatar")]
        public string AvatarUrl { get; set; }

        public bool ShouldSerializeAvatarUrl() => this.AvatarUrl != null;

    }

    [JsonObject]
    public class BaseCreateModel : BaseModel {

        [BsonElement("creator")]
        [JsonProperty("creator")]
        public BaseCreator Creator { get; set; }

    }

    public class BaseImage {

        [BsonElement("is_file")]
        [JsonProperty("is_file")]
        public bool IsFile { get; set; }

        [BsonElement("base_64")]
        [JsonProperty("base_64")]
        public string Base64 { get; set; }
        public bool ShouldSerializeBase64() => this.Base64 != null;

        [BsonElement("width")]
        [JsonProperty("width")]
        public int? Width { get; set; }
        public bool ShouldSerializeWidth() => this.Width != null;

        [BsonElement("height")]
        [JsonProperty("height")]
        public int? Height { get; set; }
        public bool ShouldSerializeHeight() => this.Height != null;

        [BsonElement("url")]
        [JsonProperty("url")]
        public string OriginUrl { get; set; }
        public bool ShouldSerializeOriginUrl() => this.OriginUrl != null;

        [BsonElement("thumb")]
        [JsonProperty("thumb")]
        public string Thumbnail { get; set; }
        public bool ShouldSerializeThumbnail() => this.Thumbnail != null;

    }

    [JsonObject]
    public class AccessControllableBase : BaseCreateModel {
        [BsonElement("state")]
        [JsonProperty("state")]
        public virtual EntryState State { get; set; } = EntryState.Published;
    }

    [JsonObject]
    public class ContentEntryBase : CanCommentModel {

        public override EntryState State { get; set; } = EntryState.Unpublished;

        [BsonElement("content")]
        [JsonProperty("details")]
        public string Content { get; set; }

        [BsonElement("cover")]
        [JsonProperty("cover")]
        public string Cover { get; set; }

    }

}
