using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models {

    public class Status {

        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("creator")]
        public StatusCreator Creator { get; set; }

        [BsonElement("likes")]
        public IEnumerable<StatusCreator> Likes { get; set; } = new List<StatusCreator>();

        [BsonElement("comments")]
        public IEnumerable<StatusComment> Comments { get; set; } = new List<StatusComment>();

        [BsonElement("c_time")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    }

    public class StatusCreator {

        [BsonElement("uuid")]
        public string UUID { get; set; }

        [BsonElement("name")]
        public string UserName { get; set; }

    }

    public class StatusComment {

        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("creator")]
        public StatusCreator Creator { get; set; }

        [BsonElement("c_time")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    }

}
