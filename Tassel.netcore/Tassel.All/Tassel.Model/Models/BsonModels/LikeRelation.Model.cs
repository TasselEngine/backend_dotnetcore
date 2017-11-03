using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Utils;

namespace Tassel.Model.Models.BsonModels {

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
    public class BaseLikesModel : BaseCreateModel {

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
}
