using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Utils;

namespace Tassel.Model.Models {

    [JsonObject]
    public class Status {

        [BsonId]
        [JsonProperty("id")]
        public string ID { get; set; } = IdentityProvider.CreateGuid();

        [BsonElement("content")]
        [JsonProperty("details")]
        public string Content { get; set; }

        [BsonElement("creator")]
        [JsonProperty("creator")]
        public StatusCreator Creator { get; set; }

        [BsonElement("likes")]
        [JsonProperty("like_users")]
        public IEnumerable<StatusCreator> Likes { get; set; } = new List<StatusCreator>();

        [BsonElement("comments")]
        [JsonProperty("comments")]
        public IEnumerable<StatusComment> Comments { get; set; } = new List<StatusComment>();

        [BsonElement("c_time")]
        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    }

    [JsonObject]
    public class StatusCreator {

        [BsonElement("uuid")]
        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [BsonElement("name")]
        [JsonProperty("user_name")]
        public string UserName { get; set; }

    }

    [JsonObject]
    public class StatusComment {

        [BsonId]
        [JsonProperty("comment_id")]
        public ObjectId ID { get; set; }

        [BsonElement("content")]
        [JsonProperty("details")]
        public string Content { get; set; }

        [BsonElement("creator")]
        [JsonProperty("creator")]
        public StatusCreator Creator { get; set; }

        [BsonElement("c_time")]
        [JsonProperty("create_time")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    }

}
