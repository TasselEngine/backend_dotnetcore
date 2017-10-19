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
        Status = 13
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
    }

    [JsonObject]
    public class BaseModel {

        [BsonId]
        [JsonProperty("id")]
        public virtual string ID { get; set; } = IdentityProvider.CreateGuid(GuidType.N);

        [BsonElement("type")]
        [JsonIgnore]
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

    }

    [JsonObject]
    public class BaseCreateModel: BaseModel {

        [BsonElement("creator")]
        [JsonProperty("creator")]
        public BaseCreator Creator { get; set; }

    }

    [JsonObject]
    public class BaseLikesModel : BaseCreateModel {

        [BsonElement("liker_ids")]
        [JsonIgnore]
        public IList<string> LikerIDs { get; set; } = new List<string>();

        [BsonIgnore]
        [JsonProperty("like_users")]
        public IList<LikesEntry> Likes { get; set; } = new List<LikesEntry>();

        [BsonIgnore]
        [JsonProperty("likers_count")]
        public int LikesCount { get => this.Likes.Count > 0 ? this.Likes.Count : this.LikerIDs.Count; }

    }

    [JsonObject]
    public class Comment : BaseCreateModel {

        [BsonElement("content")]
        [JsonProperty("details")]
        public string CommentContent { get; set; }

        public override ModelType Type { get; } = ModelType.Comment;

        [BsonElement("parent_type")]
        [JsonIgnore]
        public ModelType ParentType { get; set; } = ModelType.Default;

        [BsonElement("parent_id")]
        [JsonIgnore]
        public string ParentID { get; set; }

    }

    public class BaseImage {

        [BsonElement("is_file")]
        [JsonProperty("is_file")]
        public bool IsFile { get; set; }

        [BsonElement("url")]
        [JsonProperty("url")]
        public string Url { get; set; }
        public bool ShouldSerializeUrl() => !this.IsFile;

        [BsonIgnore]
        [JsonIgnore]
        public double FileSize { get; set; }

        [BsonElement("base_64")]
        [JsonProperty("base_64")]
        public string Base64 { get; set; }
        public bool ShouldSerializeBase64() => this.IsFile;

        [BsonElement("thumb")]
        [JsonProperty("thumb")]
        public string Thumbnail { get; set; }
        public bool ShouldSerializeThumbnail() => this.IsFile && this.Thumbnail != null;

    }

    [JsonObject]
    public class LikesEntry : BaseModel {

        [JsonIgnore]
        public override string ID { get; set; } = IdentityProvider.CreateGuid(GuidType.N);

        public override ModelType Type { get; } = ModelType.LikeEntry;

        [BsonElement("target_type")]
        [JsonIgnore]
        public ModelType TargetType { get; set; } = ModelType.Default;

        [BsonElement("parent_id")]
        [JsonIgnore]
        public string ParentID { get; set; }

        [BsonElement("user")]
        [JsonProperty("user")]
        public BaseCreator User { get; set; }

        [JsonIgnore]
        public override long? UpdateTime { get; set; }

    }

    [JsonObject]
    public class DeleteSafelyBase : BaseLikesModel {

        [BsonElement("state")]
        [JsonProperty("state")]
        public EntryState State { get; set; } = EntryState.Unpublished;

        [BsonElement("content")]
        [JsonProperty("details")]
        public string Content { get; set; }

        [BsonElement("cover")]
        [JsonProperty("cover")]
        public string Cover { get; set; }

        [BsonElement("lcomment_ids")]
        [JsonIgnore]
        public IList<string> CommentIDs { get; set; } = new List<string>();

        [BsonIgnore]
        [JsonProperty("comments")]
        public IList<Comment> Comments { get; set; } = new List<Comment>();

        [BsonIgnore]
        [JsonProperty("comments_count")]
        public int CommentsCount { get => this.Comments.Count > 0 ? this.Comments.Count : this.CommentIDs.Count; }

    }

}
