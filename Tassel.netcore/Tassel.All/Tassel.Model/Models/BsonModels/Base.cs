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

    public interface IBaseModel {
        string ID { get; set; }
        ModelType Type { get; }
        long CreateTime { get; set; }
        long? UpdateTime { get; set; }
    }

    [JsonObject]
    public class BaseModel : IBaseModel {

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

    public interface IBaseCreateModel<T>: IBaseModel {
        T Creator { get; set; }
    }

    [JsonObject]
    public class BaseCreateModel : BaseModel, IBaseCreateModel<BaseCreator> {

        [BsonElement("creator")]
        [JsonProperty("creator")]
        public BaseCreator Creator { get; set; }

    }

    public interface IAccessControllableBase: IBaseCreateModel<BaseCreator> {
        EntryState State { get; set; }
    }

    [JsonObject]
    public class AccessControllableBase : BaseCreateModel, IAccessControllableBase {
        [BsonElement("state")]
        [JsonProperty("state")]
        public virtual EntryState State { get; set; } = EntryState.Published;
    }

    public interface IBaseLikeModel<T> : IAccessControllableBase where T : ILikeable<BaseCreator> {
        IList<string> LikerIDs { get; set; }
        IList<T> Likes { get; set; }
        int LikesCount { get; }
    }

    [JsonObject]
    public class BaseLikesModel : AccessControllableBase, IBaseLikeModel<LikesEntry> {

        [BsonElement("liker_ids")]
        [JsonProperty("liker_ids")]
        public IList<string> LikerIDs { get; set; } = new List<string>();

        public bool ShouldSerializeLikerIDs() => this.Likes.Count == 0;

        [BsonIgnore]
        [JsonProperty("like_users")]
        public IList<LikesEntry> Likes { get; set; } = new List<LikesEntry>();

        public bool ShouldSerializeLikes() => this.Likes.Count > 0;

        [BsonIgnore]
        [JsonProperty("likers_count")]
        public int LikesCount { get => this.Likes.Count > 0 ? this.Likes.Count : this.LikerIDs.Count; }

    }

    public interface ICanCommentModel<T>: IBaseLikeModel<LikesEntry> where T : IComment<BaseCreator> {
        IList<string> CommentIDs { get; set; }
        IList<T> Comments { get; set; }
        int CommentsCount { get; }
    }

    public class CanCommentModel : BaseLikesModel , ICanCommentModel<Comment> {

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

    public interface IContentEntryBase : ICanCommentModel<Comment> {
        string Content { get; set; }
        string Cover { get; set; }
    }

    [JsonObject]
    public class ContentEntryBase : CanCommentModel , IContentEntryBase {

        public override EntryState State { get; set; } = EntryState.Unpublished;

        [BsonElement("content")]
        [JsonProperty("details")]
        public string Content { get; set; }

        [BsonElement("cover")]
        [JsonProperty("cover")]
        public string Cover { get; set; }

    }

}
