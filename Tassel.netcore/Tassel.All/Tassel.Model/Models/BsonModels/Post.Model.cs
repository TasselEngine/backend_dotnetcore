using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models.BsonModels {

    [JsonObject]
    public class Post : ContentEntryBase {

        public override ModelType Type { get; } = ModelType.Post;

        [BsonElement("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [BsonElement("key")]
        [JsonProperty("key")]
        public string PlainKey { get; set; }

        [BsonElement("abstract")]
        [JsonProperty("abstract")]
        public string Abstract { get; set; }

        [BsonElement("year")]
        [JsonProperty("year")]
        public int YeadStamp { get; set; }

        [BsonElement("month")]
        [JsonProperty("month")]
        public int MonthStamp { get; set; }

        [BsonElement("day")]
        [JsonProperty("day")]
        public int DayStamp { get; set; }

    }

}
