using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models.BsonModels {

    [JsonObject]
    public class WeiboDBUser : BaseModel{

        public override ModelType Type => ModelType.WeiboUser;

        [BsonElement("uid")]
        [JsonProperty("uid")]
        public string UID { get; set; }

        [BsonElement("acc_tk")]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [BsonElement("sc_nm")]
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [BsonElement("desc")]
        [JsonProperty("desc")]
        public string Description { get; set; }

        [BsonElement("dman")]
        [JsonProperty("domain")]
        public string Domain { get; set; }

        [BsonElement("avt")]
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [BsonElement("cv_mb")]
        [JsonProperty("cover_image_phone")]
        public string CoverMobile { get; set; }

        [BsonElement("cv")]
        [JsonProperty("cover_image")]
        public string Cover { get; set; }

        [JsonProperty("c_time")]
        public override DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [JsonProperty("u_time")]
        public override DateTime? UpdateTime { get; set; }
    }

}
