﻿using MongoDB.Bson.Serialization.Attributes;
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

}
