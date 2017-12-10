using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models.BsonModels {

    public interface IComment<TUser> : IAccessControllableBase {
        string CommentContent { get; set; }
        ModelType ParentType { get; set; }
        string ParentID { get; set; }
        TUser Mentioned { get; set; }
    }

    [JsonObject]
    public class Comment : AccessControllableBase, IComment<BaseCreator> {

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

        [BsonElement("mentioned")]
        [JsonProperty("mentioned")]
        public BaseCreator Mentioned { get; set; }

        public bool ShouldSerializeMentioned() => this.Mentioned != null;

        [BsonElement("replies")]
        [JsonProperty("replies")]
        public IList<Comment> Comments { get; set; } = new List<Comment>();

    }
}
