using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models.BsonModels {

    [JsonObject]
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

}
