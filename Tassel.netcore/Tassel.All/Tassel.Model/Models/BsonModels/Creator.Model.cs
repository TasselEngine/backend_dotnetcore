using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models.BsonModels {

    public interface IBaseCreator {
        string UUID { get; set; }
        string UserName { get; set; }
        string AvatarUrl { get; set; }
    }

    [JsonObject]
    public class BaseCreator: IBaseCreator {

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

}
